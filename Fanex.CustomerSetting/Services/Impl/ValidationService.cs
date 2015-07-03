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
            var levelValidationResult = ValidateActorLevel(_settingContext.Actor.Level, customerLevel);
            if (!levelValidationResult.IsSuccessful)
            {
                return levelValidationResult;
            }

            var permissionValidationResult = ValidateActorPermission(customerId, customerLevel);
            if (!permissionValidationResult.IsSuccessful)
            {
                return permissionValidationResult;
            }

            return ValidateUplineLevelIncaseOfEditingMember(customerLevel, uplineLevel);
        }

        private IResult ValidateActorLevel(UserLevel actorLevel, CustomerLevel customerLevel)
        {
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    return ValidateActorLevelForViewingSuperSetting(actorLevel);

                case CustomerLevel.Master:
                    return ValidateActorLevelForViewingMasterSetting(actorLevel);

                case CustomerLevel.Agent:
                    return ValidateActorLevelForViewingAgentSetting(actorLevel);
            }

            return Result.Success;
        }

        private IResult ValidateActorLevelForViewingSuperSetting(UserLevel actorLevel)
        {
            if (actorLevel != UserLevel.Company)
            {
                return Result.AccessDenied;
            }

            return Result.Success;
        }

        private IResult ValidateActorLevelForViewingMasterSetting(UserLevel actorLevel)
        {
            if (actorLevel != UserLevel.Company
                && actorLevel != UserLevel.Super)
            {
                return Result.AccessDenied;
            }

            return Result.Success;
        }

        private IResult ValidateActorLevelForViewingAgentSetting(UserLevel actorLevel)
        {
            if (actorLevel != UserLevel.Company
                && actorLevel != UserLevel.Master)
            {
                return Result.AccessDenied;
            }

            return Result.Success;
        }

        private IResult ValidateActorPermission(int customerId, CustomerLevel customerLevel)
        {
            return _validationDataAccess.ValidatePermission(_settingContext.Actor.Id, customerId, (byte)customerLevel);
        }

        private IResult ValidateUplineLevelIncaseOfEditingMember(CustomerLevel customerLevel, CustomerLevel? uplineLevel)
        {
            if (customerLevel == CustomerLevel.Member && uplineLevel == null)
            {
                return new Result(ResultValue.AccessDenied, Message.UplineLevelCannotBeNullIncaseOfEdittingMember);
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
            Check.Argument.IsNotNull(updatingParam.Setting.Customer, "updatingParam.Setting.Customer");
            Check.Argument.IsNotNegativeOrZero(updatingParam.Setting.Customer.Id, "updatingParam.Setting.Customer.Id");
            Check.Argument.IsNotNull(updatingParam.Setting.SettingTypes, "updatingParam.Setting.SettingTypes");
        }

        private IResult ValidateUpdateSettingSteps(UpdatingSettingParam updatingParam)
        {
            var setting = updatingParam.Setting;
            var viewingPermissionValidationResult = ValidateViewingPermission(setting.Customer.Id, setting.Customer.Level, updatingParam.UplineLevel);
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
            Check.Argument.IsNotNull(updatingMultipleParam.Customers, "updatingMultipleParam.Customers");
        }

        private IResult ValidateUpdatingMultipleSettingSteps(UpdatingMultipleSettingParam updatingMultipleParam)
        {
            var updatingSettingValidationResult = ValidateUpdatingSetting(updatingMultipleParam);
            if (!updatingSettingValidationResult.IsSuccessful)
            {
                return updatingSettingValidationResult;
            }

            var permissionValidationResult = ValidateActorPermissionOnMultipleCustomers(
                updatingMultipleParam.Customers,
                updatingMultipleParam.Setting.Customer.Level);
            if (!permissionValidationResult.IsSuccessful)
            {
                return permissionValidationResult;
            }

            return Result.Success;
        }

        private IResult ValidateActorPermissionOnMultipleCustomers(IEnumerable<Customer> customers, CustomerLevel customerLevel)
        {
            if (customerLevel == CustomerLevel.Member)
            {
                IEnumerable<int> memberIds = customers.Select(cust => cust.Id);

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