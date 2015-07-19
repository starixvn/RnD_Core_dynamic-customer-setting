namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;

    public class ValidationService : IValidationService
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
        /// <param name="queryParam">Query information for querying customer setting.</param>
        /// <returns>Query action is valid or not.</returns>
        public virtual IResult ValidateQuerySettingAction(QuerySettingParam queryParam)
        {
            Check.Argument.IsNotNull(queryParam, "queryParam");

            var customerId = queryParam.CustomerId;
            var customerLevel = queryParam.CustomerLevel;
            var uplineLevel = queryParam.UplineLevel;
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
                LogQuerySettingError(customerId, customerLevel, uplineLevel, customerSettingException);

                return new Result(ResultValue.AccessDenied, customerSettingException.Message);
            }
            catch (Exception ex)
            {
                LogQuerySettingError(customerId, customerLevel, uplineLevel, new CustomerSettingException(ex.Message, ex));

                return Result.SystemError;
            }

            return Result.Success;
        }

        private void CheckActorStatus(User actor)
        {
            var enabledStatus = _dynamicRepository.Get<bool>(new ActorStatusQueryCriteria
                {
                    ActorId = actor.Id
                });

            if (!enabledStatus)
            {
                throw new CustomerSettingException(string.Format(Message.AccountWasDisabled, actor.Name));
            }
        }

        private void CheckActorAndCustomerLevelsConstrain(User actor, Level customerLevel)
        {
            var isValidConstrain = actor.IsCompany
                || customerLevel.IsMemberLevel
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
                && customerLevel.IsMemberLevel
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
                var isBelonging = _validationDataAccess.ValidateBelonging(actor.Id, customerId);

                if (!isBelonging)
                {
                    throw new CustomerSettingException(Result.AccessDenied.Message);
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

        private void LogQuerySettingError(int customerId, Level customerLevel, Level uplineLevel, CustomerSettingException ex)
        {
            ex.SetSettingContextInfo("Validate Query Setting", _settingContext);
            ex.SetCustomerInfo(customerId, customerLevel, uplineLevel);

            _logger.Error(ex.Message, ex);
        }

        /// <summary>
        /// Validating update action, run before update, if result is valid then run update
        /// customer setting to database.
        /// This function was marked virtual in order to make it testable when test update multiple action.
        /// </summary>
        /// <param name="updateParam">Customer setting information to update to database.</param>
        /// <returns>Customer setting is valid for update to database or not.</returns>
        public virtual IResult ValidateUpdateSettingAction(UpdateSettingParam updateParam)
        {
            Check.Argument.IsNotNull(updateParam, "updateParam");

            var customerId = updateParam.Setting.CustomerId;
            var customerLevel = updateParam.Setting.CustomerLevel;
            var productId = updateParam.Setting.ProductId;
            var uplineLevel = updateParam.UplineLevel;
            var settingTypes = updateParam.Setting.SettingTypes;

            try
            {
                CheckQueryPermission(customerId, customerLevel, productId, uplineLevel);

                CheckActorLevelHasUpdatePermissionInCurrentSystem();

                CheckCustomerSettingTypesIsValidWithProductProfile(productId, settingTypes);

                CheckSettingTypesAreValid(settingTypes);

                return Result.Success;
            }
            catch (CustomerSettingException customerSettingException)
            {
                LogUpdateSettingError(updateParam, customerSettingException);

                return new Result(ResultValue.AccessDenied, customerSettingException.Message);
            }
            catch (Exception ex)
            {
                LogUpdateSettingError(updateParam, new CustomerSettingException(ex.Message, ex));

                return Result.SystemError;
            }
        }

        private void CheckQueryPermission(int customerId, Level customerLevel, byte productId, Level uplineLevel)
        {
            var querySettingParam = new QuerySettingParam(customerId, customerLevel, productId, uplineLevel);
            var result = ValidateQuerySettingAction(querySettingParam);

            if (!result.IsSuccessful)
            {
                throw new CustomerSettingException(result.Message);
            }
        }

        private void CheckActorLevelHasUpdatePermissionInCurrentSystem()
        {
            var currentSystemId = GetCurrentSystemId();

            if (_settingContext.Actor.IsCompany && currentSystemId == (byte)SystemType.Agency)
            {
                throw new CustomerSettingException(Result.AccessDenied.Message);
            }
        }

        /// <summary>
        /// Gets the system identifier, declares virtual for being testable.
        /// </summary>
        /// <returns>Current system id which actor performs actions.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        public virtual byte GetCurrentSystemId()
        {
            return CustomerSettingSection.Settings.SystemId;
        }

        private void CheckCustomerSettingTypesIsValidWithProductProfile(byte productId, IEnumerable<SettingType> customerSettingTypes)
        {
            var productSettingTypes = _productService.GetProductSettingTypes(productId);

            foreach (var customerSettingType in customerSettingTypes)
            {
                if (!productSettingTypes.Any(stt => stt.Id == customerSettingType.Id))
                {
                    throw new CustomerSettingException(Result.AccessDenied.Message);
                }
            }
        }

        private void CheckSettingTypesAreValid(IEnumerable<SettingType> settingTypes)
        {
            foreach (SettingType settingType in settingTypes)
            {
                var result = settingType.Validate();

                if (!result.IsSuccessful)
                {
                    throw new CustomerSettingException(result.Message);
                }
            }
        }

        private void LogUpdateSettingError(UpdateSettingParam updateParam, CustomerSettingException ex)
        {
            ex.SetSettingContextInfo("Validate Update Setting", _settingContext);
            ex.SetCustomerInfo(updateParam.Setting.CustomerId, updateParam.Setting.CustomerLevel, updateParam.UplineLevel);
            ex.SetProductInfo(updateParam.Setting.ProductId);

            _logger.Error(ex.Message, ex);
        }

        public IResult ValidateUpdateMultipleSettingAction(UpdateMultipleSettingParam updateMultipleParam)
        {
            Check.Argument.IsNotNull(updateMultipleParam, "updateMultipleParam");

            try
            {
                CheckUpdateSetting(updateMultipleParam);

                CheckActorPermissionOnMultipleMembers(updateMultipleParam.CustomerIds, updateMultipleParam.Setting.CustomerLevel);

                return Result.Success;
            }
            catch (CustomerSettingException customerSettingException)
            {
                LogUpdateMultipleSettingError(updateMultipleParam, customerSettingException);

                return new Result(ResultValue.AccessDenied, customerSettingException.Message);
            }
            catch (Exception ex)
            {
                LogUpdateMultipleSettingError(updateMultipleParam, new CustomerSettingException(ex.Message, ex));

                return Result.SystemError;
            }
        }

        private void CheckUpdateSetting(UpdateSettingParam updateParam)
        {
            var result = ValidateUpdateSettingAction(updateParam);

            if (!result.IsSuccessful)
            {
                throw new CustomerSettingException(result.Message);
            }
        }

        private void CheckActorPermissionOnMultipleMembers(IEnumerable<int> memberIds, Level customerLevel)
        {
            if (customerLevel.IsMemberLevel)
            {
                var result = _validationDataAccess.ValidatePermissionMultiple(_settingContext.Actor.Id, string.Join(",", memberIds));

                if (!result.IsSuccessful)
                {
                    throw new CustomerSettingException(result.Message);
                }
            }
        }

        private void LogUpdateMultipleSettingError(UpdateMultipleSettingParam updateParam, CustomerSettingException ex)
        {
            ex.SetSettingContextInfo("Validate Update Multiple Setting", _settingContext);
            ex.SetCustomerInfo(
                string.Join(",", updateParam.CustomerIds),
                updateParam.Setting.CustomerLevel,
                updateParam.UplineLevel);
            ex.SetProductInfo(updateParam.Setting.ProductId);

            _logger.Error(ex.Message, ex);
        }
    }
}