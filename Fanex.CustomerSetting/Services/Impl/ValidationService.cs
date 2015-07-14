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
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.NAP.Framework.Data;

    public class ValidationService : IValidationService
    {
        private readonly ISettingContext _settingContext;
        private readonly ILoggingService _loggingService;
        private readonly IProductService _productService;
        private readonly IValidationDataAccess _validationDataAccess;
        private readonly IDynamicRepository _dynamicRepository;

        public ValidationService(
            ISettingContext settingContext,
            IProductService productService = null,
            ILoggingService loggingService = null,
            IValidationDataAccess validationDataAccess = null,
            IDynamicRepository dynamicRepository = null)
        {
            _settingContext = settingContext;
            _productService = productService ?? new ProductService();
            _loggingService = loggingService ?? new LoggingService(_settingContext);
            _validationDataAccess = validationDataAccess ?? new ValidationDataAccess();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public IResult ValidateQuerySetting(QuerySettingParam queryParam)
        {
            Check.Argument.IsNotNull(queryParam, "queryParam");

            var customerId = queryParam.CustomerId;
            var customerLevel = queryParam.CustomerLevel;
            var uplineLevel = queryParam.UplineLevel;

            IResult result = VerifyActorStatusIsEnabled();

            if (!result.IsSuccessful)
            {
                return result;
            }

            if (!IsCustomerLevelEqualWithCustomerLevelStoredInDatabase(customerId, customerLevel)
                || !IsLevelConstrainsBetweenActorAndCustomerValid(customerLevel))
            {
                return Result.AccessDenied;
            }

            result = ValidateActorLevelWithUplineLevelOfCustomer(customerLevel, uplineLevel);

            if (!result.IsSuccessful)
            {
                return result;
            }

            return ValidateBelongIn(customerId);
        }

        private bool IsLevelConstrainsBetweenActorAndCustomerValid(CustomerLevel customerLevel)
        {
            var actorLevel = _settingContext.Actor.Level;

            var result = actorLevel == UserLevel.Company
                || (byte)customerLevel == Level.Member
                || actorLevel.IsDirectUplineLevelOf(customerLevel);

            return result;
        }

        private IResult VerifyActorStatusIsEnabled()
        {
            var isEnabled = _dynamicRepository.Get<bool>(new ActorStatusQueryCriteria
                {
                    ActorId = _settingContext.Actor.Id
                });

            if (!isEnabled)
            {
                return new Result(ResultValue.AccessDenied, string.Format(Message.AccountWasDisabled, _settingContext.Actor.Name));
            }

            return Result.Success;
        }

        private IResult ValidateBelongIn(int customerId)
        {
            if (_settingContext.Actor.Level == UserLevel.Company)
            {
                return Result.Success;
            }
            else
            {
                return _validationDataAccess.ValidateBelongIn(_settingContext.Actor.Id, customerId);
            }
        }

        private bool IsCustomerLevelEqualWithCustomerLevelStoredInDatabase(int customerId, CustomerLevel customerLevel)
        {
            var actualCustomerLevel = _dynamicRepository.Get<byte>(new CustomerLevelQueryCriteria
            {
                CustId = customerId
            });

            return (CustomerLevel)actualCustomerLevel == customerLevel;
        }

        private IResult ValidateActorLevelWithUplineLevelOfCustomer(CustomerLevel customerLevel, CustomerLevel? uplineLevel)
        {
            //// If actor is not company when edit member, actor level should equal with upline level.
            if (_settingContext.Actor.Level != UserLevel.Company
                && customerLevel == CustomerLevel.Member
                && uplineLevel != null
                && _settingContext.Actor.Level.Value != (int)uplineLevel.Value)
            {
                return new Result(ResultValue.AccessDenied, Message.UplineLevelIsNotValid);
            }

            return Result.Success;
        }

        public IResult ValidateUpdatingSetting(UpdatingSettingParam updatingParam)
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
            catch (Exception ex)
            {
                _loggingService.LogValidatingUpdatingError(
                    updatingParam.Setting.CustomerId,
                    updatingParam.UplineLevel,
                    updatingParam.Setting.ProductId,
                    ex);

                return Result.SystemError;
            }
        }

        private IResult ValidateActorLevelWithCurrentSystem()
        {
            var currentSystemId = GetCurrentSystemId();

            if (_settingContext.Actor.Level == UserLevel.Company
                && currentSystemId == (byte)SystemType.Agency)
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
                _loggingService.LogValidatingUpdatingMultipleError(
                    updatingMultipleParam.CustomerIds,
                    updatingMultipleParam.UplineLevel,
                    updatingMultipleParam.Setting.ProductId,
                    ex);

                return Result.SystemError;
            }
        }

        private IResult ValidateActorPermissionOnMultipleCustomers(IEnumerable<int> memberIds, CustomerLevel customerLevel)
        {
            if (customerLevel == CustomerLevel.Member)
            {
                return _validationDataAccess.ValidatePermissionMultiple(_settingContext.Actor.Id, string.Join(",", memberIds));
            }

            return Result.Success;
        }
    }
}