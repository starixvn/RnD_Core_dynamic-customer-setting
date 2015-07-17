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
        /// updating action, this function mark virtual for testable updating action validation.
        /// </summary>
        /// <param name="queryParam">Query information for querying customer setting.</param>
        /// <returns>Query action is valid or not.</returns>
        public virtual IResult ValidateQuerySetting(QuerySettingParam queryParam)
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
            catch (CustomerSettingException ex)
            {
                _logger.Error(ex.Message, ex);

                return new Result(ResultValue.AccessDenied, ex.Message);
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

        private void CheckActorAndCustomerLevelsConstrain(User actor, UserLevel customerLevel)
        {
            var isValidConstrain = actor.IsCompany
                || customerLevel.IsMemberLevel
                || actor.Level.IsDirectUplineLevelOf(customerLevel);

            if (!isValidConstrain)
            {
                throw new CustomerSettingException();
            }
        }

        private void CheckActorLevelAndCustomerUplineLevelConstrain(User actor, UserLevel customerLevel, UserLevel uplineLevel)
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
                    throw new CustomerSettingException();
                }
            }
        }

        /// <summary>
        /// Check whether input customer level is equal to value stored in database
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customerLevel"></param>
        private void CheckCustomerLevel(int customerId, UserLevel customerLevel)
        {
            var actualCustomerLevel = _dynamicRepository.Get<byte>(new CustomerLevelQueryCriteria
            {
                CustId = customerId
            });

            if (actualCustomerLevel != customerLevel.Value)
            {
                throw new CustomerSettingException();
            }
        }

        /// <summary>
        /// Validating updating action, run before updating, if result is valid then run updating
        /// customer setting to database.
        /// This function was marked virtual in order to make it testable when test updating multiple action.
        /// </summary>
        /// <param name="updatingParam">Customer setting information to update to database.</param>
        /// <returns>Customer setting is valid for updating to database or not.</returns>
        public virtual IResult ValidateUpdatingSetting(UpdatingSettingParam updatingParam)
        {
            Check.Argument.IsNotNull(updatingParam, "updatingParam");

            try
            {
                var setting = updatingParam.Setting;
                var querySettingParam = new QuerySettingParam(setting.CustomerId, setting.CustomerLevel, setting.ProductId, updatingParam.UplineLevel);
                var result = ValidateQuerySetting(querySettingParam);

                if (!result.IsSuccessful)
                {
                    return result;
                }

                result = ValidateActorLevelWithCurrentSystem();

                if (!result.IsSuccessful)
                {
                    return result;
                }

                result = ValidateProductProfile(setting.ProductId, setting.SettingTypes);

                if (!result.IsSuccessful)
                {
                    return result;
                }

                return ValidateSettingTypes(setting.SettingTypes);
            }
            catch (CustomerSettingException ex)
            {
                ex.ProductId = updatingParam.Setting.ProductId;
                ex.CustomerId = updatingParam.Setting.CustomerId;
                
                //// _logger.LogValidatingUpdatingError(
                ////    updatingParam.Setting.CustomerId,
                ////    updatingParam.UplineLevel,
                ////    updatingParam.Setting.ProductId,
                ////    ex);

                _logger.Error(ex.Message, ex);

                return Result.SystemError;
            }
        }

        private IResult ValidateActorLevelWithCurrentSystem()
        {
            var currentSystemId = GetCurrentSystemId();

            if (_settingContext.Actor.IsCompany && currentSystemId == (byte)SystemType.Agency)
            {
                return Result.AccessDenied;
            }

            return Result.Success;
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

        private IResult ValidateProductProfile(byte productId, IEnumerable<SettingType> customerSettingTypes)
        {
            var productSettingTypes = _productService.GetProductSettingTypes(productId);

            foreach (var customerSettingType in customerSettingTypes)
            {
                if (!productSettingTypes.Any(stt => stt.Id == customerSettingType.Id))
                {
                    return Result.AccessDenied;
                }
            }

            return Result.Success;
        }

        private IResult ValidateSettingTypes(IEnumerable<SettingType> settingTypes)
        {
            foreach (SettingType settingType in settingTypes)
            {
                var validationResult = settingType.Validate();

                if (!validationResult.IsSuccessful)
                {
                    return validationResult;
                }
            }

            return Result.Success;
        }

        public IResult ValidateUpdatingMultipleSetting(UpdatingMultipleSettingParam updatingMultipleParam)
        {
            Check.Argument.IsNotNull(updatingMultipleParam, "updatingMultipleParam");

            try
            {
                var result = ValidateUpdatingSetting(updatingMultipleParam);

                if (!result.IsSuccessful)
                {
                    return result;
                }

                return ValidateActorPermissionOnMultipleCustomers(
                    updatingMultipleParam.CustomerIds,
                    updatingMultipleParam.Setting.CustomerLevel);
            }
            catch (Exception ex)
            {
                //// _logger.LogValidatingUpdatingMultipleError(
                ////    updatingMultipleParam.CustomerIds,
                ////    updatingMultipleParam.UplineLevel,
                ////    updatingMultipleParam.Setting.ProductId,
                ////    ex);

                _logger.Error(ex.Message, ex);

                return Result.SystemError;
            }
        }

        private IResult ValidateActorPermissionOnMultipleCustomers(IEnumerable<int> memberIds, UserLevel customerLevel)
        {
            if (customerLevel.IsMemberLevel)
            {
                return _validationDataAccess.ValidatePermissionMultiple(_settingContext.Actor.Id, string.Join(",", memberIds));
            }

            return Result.Success;
        }
    }
}