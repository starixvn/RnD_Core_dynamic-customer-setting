namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.Logging;

    public class ValidationService : IValidationService
    {
        private readonly ISettingContext _settingContext;
        private readonly IValidationDataAccess _validationDataAccess;
        private readonly IProductService _productService;
        private readonly ILogger _logger;

        public ValidationService(
            ISettingContext settingContext,
            IValidationDataAccess validationDataAccess = null,
            IProductService productService = null,
            ILogger logger = null)
        {
            _settingContext = settingContext;
            _validationDataAccess = validationDataAccess ?? new ValidationDataAccess();
            _productService = productService ?? new ProductService();
            _logger = logger ?? Logger.Log;
        }

        public IResult ValidateQuerySetting(QuerySettingParam queryParam)
        {
            Check.Argument.IsNotNull(queryParam, "queryParam");
            Check.Argument.IsNotNegativeOrZero(queryParam.CustomerId, "queryParam.CustomerId");
            Check.Argument.IsNotNegativeOrZero((byte)queryParam.CustomerLevel, "queryParam.CustomerLevel");

            return ValidateViewingPermission(queryParam.CustomerId, queryParam.CustomerLevel, queryParam.UplineLevel);
        }

        private IResult ValidateViewingPermission(int customerId, CustomerLevel customerLevel, CustomerLevel? uplineLevel)
        {
            IResult result = ValidateActorLevelForViewingCustomerSetting(customerLevel);
            if (!result.IsSuccessful)
            {
                return result;
            }

            result = ValidateActorEnabled();
            if (!result.IsSuccessful)
            {
                return result;
            }

            result = ValidateCustomerAndActorLevel(customerId, customerLevel);
            if (!result.IsSuccessful)
            {
                return result;
            }

            result = ValidateBelongIn(customerId);
            if (!result.IsSuccessful)
            {
                return result;
            }

            return ValidateUplineLevel(customerLevel, uplineLevel);
        }

        private IResult ValidateActorLevelForViewingCustomerSetting(CustomerLevel customerLevel)
        {
            UserLevel actorLevel = _settingContext.Actor.Level;
            bool isValid = true;
            switch (actorLevel)
            {
                case UserLevel.Company:
                    break;
                case UserLevel.Super:
                    if ((byte)customerLevel >= (byte)UserLevel.Super 
                        || customerLevel == CustomerLevel.Agent)
                    {
                        isValid = false;
                    }

                    break;

                case UserLevel.Master:
                    if ((byte)customerLevel >= (byte)UserLevel.Master)
                    {
                        isValid = false;
                    }

                    break;

                case UserLevel.Agent:
                    if ((byte)customerLevel >= (byte)UserLevel.Agent)
                    {
                        isValid = false;
                    }

                    break;
            }

            return isValid ? Result.Success : Result.AccessDenied;
        }

        private IResult ValidateActorEnabled()
        {
            return _validationDataAccess.ValidateActorEnabled(_settingContext.Actor.Id);
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

        private IResult ValidateCustomerAndActorLevel(int customerId, CustomerLevel customerLevel)
        {
            return _validationDataAccess.ValidateLevel(_settingContext.Actor.Id, customerId, (byte)customerLevel);
        }

        private IResult ValidateUplineLevel(CustomerLevel customerLevel, CustomerLevel? uplineLevel)
        {
            if (customerLevel == CustomerLevel.Member)
            {
                return ValidateUplineLevelIncaseOfEditingMember(uplineLevel);
            }
            else
            {
                if (uplineLevel.HasValue)
                {
                    return new Result(ResultValue.AccessDenied, Message.UplineLevelShouldBeNull);
                }

                return Result.Success;
            }
        }

        private IResult ValidateUplineLevelIncaseOfEditingMember(CustomerLevel? uplineLevel)
        {
            if (uplineLevel == null)
            {
                return new Result(ResultValue.AccessDenied, Message.UplineLevelCannotBeNullIncaseOfEdittingMember);
            }

            if (_settingContext.Actor.Level != UserLevel.Company 
                && (int)_settingContext.Actor.Level != (int)uplineLevel.Value)
            {
                return new Result(ResultValue.AccessDenied, Message.UplineLevelIsNotValid);
            }

            return Result.Success;
        }

        public IResult ValidateUpdatingSetting(UpdatingSettingParam updatingParam)
        {
            try
            {
                CheckUpdatingSettingArguments(updatingParam);

                return ValidateUpdateSettingSteps(updatingParam);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return Result.SystemError;
            }
        }

        private void CheckUpdatingSettingArguments(UpdatingSettingParam updatingParam)
        {
            Check.Argument.IsNotNull(updatingParam, "updatingParam");
            Check.Argument.IsNotNull(updatingParam.Setting, "updatingParam.Setting");
            Check.Argument.IsNotNegativeOrZero(updatingParam.Setting.CustomerId, "updatingParam.Setting.CustomerId");
            Check.Argument.IsNotNull(updatingParam.Setting.SettingTypes, "updatingParam.Setting.SettingTypes");
        }

        private IResult ValidateUpdateSettingSteps(UpdatingSettingParam updatingParam)
        {
            var setting = updatingParam.Setting;
            var viewingPermissionValidationResult = ValidateViewingPermission(setting.CustomerId, setting.CustomerLevel, updatingParam.UplineLevel);
            if (!viewingPermissionValidationResult.IsSuccessful)
            {
                return viewingPermissionValidationResult;
            }

            var productProfileValidationResult = ValidateProductProfile(setting.ProductId, setting.SettingTypes);
            if (!productProfileValidationResult.IsSuccessful)
            {
                return productProfileValidationResult;
            }

            var settingTypesValidationResult = ValidateSettingTypes(setting.SettingTypes);
            if (!settingTypesValidationResult.IsSuccessful)
            {
                return settingTypesValidationResult;
            }

            return Result.Success;
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
            try
            {
                CheckUpdatingMultipleSettingArguments(updatingMultipleParam);

                return ValidateUpdatingMultipleSettingSteps(updatingMultipleParam);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return Result.SystemError;
            }
        }

        private void CheckUpdatingMultipleSettingArguments(UpdatingMultipleSettingParam updatingMultipleParam)
        {
            Check.Argument.IsNotNull(updatingMultipleParam, "updatingMultipleParam");
            Check.Argument.IsNotNull(updatingMultipleParam.Setting, "updatingMultipleParam.Setting");
            Check.Argument.IsNotNull(updatingMultipleParam.CustomerIds, "updatingMultipleParam.Customers");
        }

        private IResult ValidateUpdatingMultipleSettingSteps(UpdatingMultipleSettingParam updatingMultipleParam)
        {
            var updatingSettingValidationResult = ValidateUpdatingSetting(updatingMultipleParam);
            if (!updatingSettingValidationResult.IsSuccessful)
            {
                return updatingSettingValidationResult;
            }

            var permissionValidationResult = ValidateActorPermissionOnMultipleCustomers(
                updatingMultipleParam.CustomerIds,
                updatingMultipleParam.Setting.CustomerLevel);
            if (!permissionValidationResult.IsSuccessful)
            {
                return permissionValidationResult;
            }

            return Result.Success;
        }

        private IResult ValidateActorPermissionOnMultipleCustomers(IEnumerable<int> memberIds, CustomerLevel customerLevel)
        {
            if (customerLevel == CustomerLevel.Member)
            {
                var validationResult = _validationDataAccess.ValidatePermissionMultiple(_settingContext.Actor.Id, string.Join(",", memberIds));

                return validationResult;
            }

            return Result.Success;
        }

        private void LogError(Exception ex)
        {
            _logger.Error(ex.Message, ex);
        }
    }
}