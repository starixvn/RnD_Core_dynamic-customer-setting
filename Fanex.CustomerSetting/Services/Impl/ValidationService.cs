namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Constants;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Resource;
    using Fanex.Data.Repository;
    using Fanex.Logging;

    internal class ValidationService : IValidationService
    {
        private readonly ISettingContext _settingContext;
        private readonly IProductService _productService;
        private readonly IValidationDataAccess _validationDataAccess;
        private readonly IDynamicRepository _dynamicRepository;
        private readonly ILogger _logger;

        public ValidationService(
            ISettingContext settingContext,
            IProductService productService = null,
            IValidationDataAccess validationDataAccess = null,
            IDynamicRepository dynamicRepository = null,
            ILogger logger = null)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");

            _settingContext = settingContext;
            _productService = productService ?? new ProductService();
            _validationDataAccess = validationDataAccess ?? new ValidationDataAccess();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
            _logger = logger ?? Logger.Log;
        }

        /// <summary>
        /// Validate Query action is valid or not, this function also using by
        /// update action, this function mark virtual for testable update action validation.
        /// </summary>
        /// <param name="query">Query information for querying customer setting.</param>
        /// <returns>Query action is valid or not.</returns>
        public virtual IResult ValidateQuerySettingAction(CustomerSettingQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var customerId = query.CustomerId;
            var customerLevel = query.CustomerLevel;
            var uplineLevel = query.UplineLevel;
            var actor = _settingContext.Actor;

            try
            {
                CheckActorStatus(actor);

                CheckCustomerLevel(customerId, customerLevel);

                CheckActorAndCustomerLevelsConstrain(actor, customerLevel);

                CheckActorLevelAndCustomerUplineLevelConstrain(actor, customerLevel, uplineLevel);

                CheckBelonging(actor, customerId);
            }
            catch (CustomerSettingException customerSettingException)
            {
                WriteQuerySettingLog(customerId, customerLevel, uplineLevel, customerSettingException);

                return new Result(ResultValue.AccessDenied, customerSettingException.OriginalMessage);
            }

            return Result.Success;
        }

        private void CheckActorStatus(User actor)
        {
            var enabledStatus = _dynamicRepository.Get<bool>(new ActorStatusQueryCriteria
                {
                    CustId = actor.Id
                });

            if (!enabledStatus)
            {
                throw new CustomerSettingException(Message.AccountWasDisabled);
            }
        }

        private void CheckActorAndCustomerLevelsConstrain(User actor, Level customerLevel)
        {
            var isValidConstrain = actor.IsCompany
                || customerLevel.IsMemberLevel()
                || actor.Level.IsDirectUplineOf(customerLevel);

            if (!isValidConstrain)
            {
                throw new CustomerSettingException(Result.AccessDenied.Message);
            }
        }

        private void CheckActorLevelAndCustomerUplineLevelConstrain(User actor, Level customerLevel, Level uplineLevel)
        {
            //// If actor is not company when edit member, actor level should equal with upline level.
            if (actor.IsNotCompany
                && customerLevel.IsMemberLevel()
                && uplineLevel.IsNot(null)
                && actor.Level.IsNot(uplineLevel))
            {
                throw new CustomerSettingException(Message.UplineLevelIsNotValid);
            }
        }

        private void CheckBelonging(User actor, int customerId)
        {
            if (actor.IsNotCompany)
            {
                var isBelonging = _dynamicRepository.Get<bool>(new CheckBelongingCriteria
                {
                    ActorId = actor.Id,
                    CustomerId = customerId
                });

                if (!isBelonging)
                {
                    throw new CustomerSettingException(Message.CustomersAreNotSameLine);
                }
            }
        }

        /// <summary>
        /// Check whether input customer level is equal to value stored in database
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customerLevel"></param>
        private void CheckCustomerLevel(int customerId, Level customerLevel)
        {
            var actualCustomerLevel = _dynamicRepository.Get<byte>(new CustomerLevelQueryCriteria
            {
                CustId = customerId
            });

            if (actualCustomerLevel != customerLevel.Value)
            {
                throw new CustomerSettingException(Result.AccessDenied.Message);
            }
        }

        private void WriteQuerySettingLog(int customerId, Level customerLevel, Level uplineLevel, CustomerSettingException ex)
        {
            ex.SetSettingContextInfo("Validate Query Setting", _settingContext);
            ex.SetCustomerInfo(customerId, customerLevel, uplineLevel);

            _logger.Warn(ex.Message, ex);
        }

        /// <summary>
        /// Validating update action, run before update, if result is valid then run update
        /// customer setting to database.
        /// This function was marked virtual in order to make it testable when test update multiple action.
        /// </summary>
        /// <param name="command">Customer setting information to update to database.</param>
        /// <returns>Customer setting is valid for update to database or not.</returns>
        public virtual IResult ValidateUpdateSettingAction(UpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, "command");

            var customerId = command.Setting.CustomerId;
            var customerLevel = command.Setting.CustomerLevel;
            var productId = command.Setting.ProductId;
            var uplineLevel = command.UplineLevel;
            var settingTypes = command.Setting.SettingTypes;

            try
            {
                CheckQueryPermission(customerId, customerLevel, productId, uplineLevel);

                CheckCustomerSettingTypesIsValidWithProductProfile(productId, settingTypes);
            }
            catch (CustomerSettingException customerSettingException)
            {
                WriteUpdateSettingLog(command, customerSettingException);

                return new Result(ResultValue.AccessDenied, customerSettingException.OriginalMessage);
            }

            return Result.Success;
        }

        private void CheckQueryPermission(int customerId, Level customerLevel, byte productId, Level uplineLevel)
        {
            var querySettingParam = new CustomerSettingQuery(customerId, customerLevel, productId, uplineLevel);
            var result = ValidateQuerySettingAction(querySettingParam);

            if (result.IsNotOk)
            {
                throw new CustomerSettingException(result.Message);
            }
        }

        private void CheckCustomerSettingTypesIsValidWithProductProfile(byte productId, IEnumerable<SettingType> customerSettingTypes)
        {
            var productSettingTypes = _productService.GetProductSettingTypes(productId, _settingContext.System)
                .Where(product => product.DisplayMode == DisplayMode.Editable);

            foreach (var customerSettingType in customerSettingTypes)
            {
                if (!productSettingTypes.Any(stt => stt.Id == customerSettingType.Id))
                {
                    throw new CustomerSettingException(Result.AccessDenied.Message);
                }
            }
        }

        private void WriteUpdateSettingLog(UpdateCustomerSettingCommand command, CustomerSettingException ex)
        {
            ex.SetSettingContextInfo("Validate Update Setting", _settingContext);
            ex.SetCustomerInfo(command.Setting.CustomerId, command.Setting.CustomerLevel, command.UplineLevel);
            ex.SetProductInfo(command.Setting.ProductId);

            _logger.Warn(ex.Message, ex);
        }

        public IResult ValidateUpdateMultipleSettingAction(MultipleUpdateCustomerSettingCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            try
            {
                CheckUpdateSetting(command);

                CheckActorPermissionOnMultipleMembers(command.CustomerIds, command.Setting.CustomerLevel);
            }
            catch (CustomerSettingException customerSettingException)
            {
                WriteUpdateMultipleSettingLog(command, customerSettingException);

                return new Result(ResultValue.AccessDenied, customerSettingException.OriginalMessage);
            }

            return Result.Success;
        }

        private void CheckUpdateSetting(UpdateCustomerSettingCommand command)
        {
            var result = ValidateUpdateSettingAction(command);

            if (result.IsNotOk)
            {
                throw new CustomerSettingException(result.Message);
            }
        }

        private void CheckActorPermissionOnMultipleMembers(IEnumerable<int> memberIds, Level customerLevel)
        {
            var result = _validationDataAccess.ValidatePermissionMultiple(_settingContext.Actor.Id, string.Join(",", memberIds));

            if (result.IsNotOk)
            {
                throw new CustomerSettingException(result.Message);
            }
        }

        private void WriteUpdateMultipleSettingLog(MultipleUpdateCustomerSettingCommand command, CustomerSettingException ex)
        {
            ex.SetSettingContextInfo("Validate Update Multiple Setting", _settingContext);
            ex.SetCustomerInfo(
                string.Join(",", command.CustomerIds),
                command.Setting.CustomerLevel,
                command.UplineLevel);
            ex.SetProductInfo(command.Setting.ProductId);

            _logger.Warn(ex.Message, ex);
        }
    }
}