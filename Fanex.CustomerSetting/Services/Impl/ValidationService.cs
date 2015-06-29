namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;

    public class ValidationService : IValidationService
    {
        private readonly ISettingContext _settingContext;
        private readonly IValidationDataAccess _validationDataAccess;
        private readonly IProductService _productService;

        public ValidationService(
            ISettingContext settingContext,
            IValidationDataAccess validationDataAccess = null,
            IProductService productService = null)
        {
            _settingContext = settingContext;
            _validationDataAccess = validationDataAccess ?? new ValidationDataAccess();
            _productService = productService ?? new ProductService();
        }

        public IResult ValidateQuerySetting(QuerySettingParams queryParams)
        {
            Check.Argument.IsNotNull(queryParams, "queryParams");
            Check.Argument.IsNotNegativeOrZero(queryParams.CustomerId, "queryParams.CustomerId");
            Check.Argument.IsNotNegativeOrZero((byte)queryParams.CustomerLevel, "queryParams.CustomerLevel");

            return ValidateViewingPermission(queryParams.CustomerId, queryParams.CustomerLevel);
        }

        private IResult ValidateViewingPermission(int customerId, CustomerLevel customerLevel)
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

            return Result.Success;
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

        public IResult ValidateUpdatingSetting(UpdatingSettingParams updatingParams)
        {
            CheckUpdatingSettingArguments(updatingParams);

            var setting = updatingParams.Setting;
            var viewingPermissionValidationResult = ValidateViewingPermission(setting.Customer.Id, setting.Customer.Level);
            if (!viewingPermissionValidationResult.IsSuccessful)
            {
                return viewingPermissionValidationResult;
            }

            var productProfileValidationResult = ValidateProductProfile(setting.Product.Id, setting.SettingTypes);
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

        private void CheckUpdatingSettingArguments(UpdatingSettingParams updatingParams)
        {
            Check.Argument.IsNotNull(updatingParams, "updatingParams");
            Check.Argument.IsNotNull(updatingParams.Setting, "updatingParams.Setting");
            Check.Argument.IsNotNull(updatingParams.Setting.Customer, "updatingParams.Setting.Customer");
            Check.Argument.IsNotNegativeOrZero(updatingParams.Setting.Customer.Id, "updatingParams.Setting.Customer.Id");
            Check.Argument.IsNotNull(updatingParams.Setting.SettingTypes, "updatingParams.Setting.SettingTypes");
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
                    return new Result(settingType.Id, settingType.Name, validationResult);
                }
            }

            return Result.Success;
        }

        public IResult ValidateUpdatingMultipleSetting(UpdatingMultipleSettingParams updatingMultipleParams)
        {
            CheckUpdatingMultipleSettingArguments(updatingMultipleParams);

            var updatingSettingValidationResult = ValidateUpdatingSetting(updatingMultipleParams);
            if (!updatingSettingValidationResult.IsSuccessful)
            {
                return updatingSettingValidationResult;
            }

            var permissionValidationResult = ValidateActorPermissionOnMultipleCustomers(
                updatingMultipleParams.Customers,
                updatingMultipleParams.Setting.Customer.Level);
            if (!permissionValidationResult.IsSuccessful)
            {
                return permissionValidationResult;
            }

            return Result.Success;
        }

        private void CheckUpdatingMultipleSettingArguments(UpdatingMultipleSettingParams updatingMultipleParams)
        {
            Check.Argument.IsNotNull(updatingMultipleParams, "updatingMultipleParams");
            Check.Argument.IsNotNull(updatingMultipleParams.Setting, "updatingMultipleParams.Setting");
            Check.Argument.IsNotNull(updatingMultipleParams.Customers, "updatingMultipleParams.Customers");
        }

        private IResult ValidateActorPermissionOnMultipleCustomers(IEnumerable<Customer> customers, CustomerLevel customerLevel)
        {
            if (customerLevel == CustomerLevel.Member)
            {
                IEnumerable<int> memberIds = customers.Select(cust => cust.Id);

                var permissionValidationResult = _validationDataAccess.ValidatePermissionMultiple(_settingContext.Actor.Id, string.Join(",", memberIds));

                return permissionValidationResult;
            }

            return Result.Success;
        }
    }
}