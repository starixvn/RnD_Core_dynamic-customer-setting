namespace Fanex.CustomerSetting.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.DataAccesses.Impl;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Dtos.Results.Impl;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.SPObjects.Commands.Validation;

    public class ValidationService : IValidationService
    {
        private ISettingContext _settingContext;
        private IValidationDataAccess _validationDataAccess;
        private IProductService _productService;

        public ValidationService()
            : this(null, null, null)
        {
        }

        public ValidationService(
            ISettingContext settingContext,
            IValidationDataAccess validationDataAccess,
            IProductService productService)
        {
            _settingContext = settingContext;
            _validationDataAccess = validationDataAccess ?? new ValidationDataAccess();
            _productService = productService ?? new ProductService();
        }

        public IResult ValidateSettingQuery(SettingQuery settingQuery)
        {
            if (settingQuery == null)
            {
                throw new ArgumentNullException("settingQuery");
            }

            return ValidateViewingPermission(settingQuery.CustomerId, settingQuery.CustomerLevel);
        }

        private IResult ValidateViewingPermission(int customerId, CustomerLevel customerLevel)
        {
            var levelValidationResult = ValidateLevel(_settingContext.Actor.Level, customerLevel);
            if (!levelValidationResult.IsSuccessful)
            {
                return levelValidationResult;
            }

            var permissionValidationResult = ValidatePermission(customerId, customerLevel);
            if (!permissionValidationResult.IsSuccessful)
            {
                return permissionValidationResult;
            }

            return Result.Success;
        }

        private IResult ValidateLevel(UserLevel actorLevel, CustomerLevel customerLevel)
        {
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    return ValidateLevelForViewingSuperSetting(actorLevel);

                case CustomerLevel.Master:
                    return ValidateLevelForViewingMasterSetting(actorLevel);

                case CustomerLevel.Agent:
                    return ValidateLevelForViewingAgentSetting(actorLevel);
            }

            return Result.Success;
        }

        private IResult ValidateLevelForViewingSuperSetting(UserLevel actorLevel)
        {
            if (actorLevel != UserLevel.Company)
            {
                return Result.AccessDenied;
            }

            return Result.Success;
        }

        private IResult ValidateLevelForViewingMasterSetting(UserLevel actorLevel)
        {
            if (actorLevel != UserLevel.Company
                && actorLevel != UserLevel.Super)
            {
                return Result.AccessDenied;
            }

            return Result.Success;
        }

        private IResult ValidateLevelForViewingAgentSetting(UserLevel actorLevel)
        {
            if (actorLevel != UserLevel.Company
                && actorLevel != UserLevel.Master)
            {
                return Result.AccessDenied;
            }

            return Result.Success;
        }

        private IResult ValidatePermission(int customerId, CustomerLevel customerLevel)
        {
            return _validationDataAccess.ValidatePermission(new PermissionValidationCommand
            {
                CustId = _settingContext.Actor.Id,
                TargetCustId = customerId,
                TargetRoleId = (int)customerLevel
            });
        }

        public IResult ValidateSettingCommand(SettingCommand settingCommand)
        {
            var viewingPermissionValidationResult = ValidateViewingPermission(
                settingCommand.Setting.Customer.Id,
                settingCommand.Setting.Customer.Level);
            if (!viewingPermissionValidationResult.IsSuccessful)
            {
                return viewingPermissionValidationResult;
            }

            var productProfileValidationResult = ValidateProductProfile(
                settingCommand.Setting.Product.Id,
                settingCommand.Setting.SettingTypes);
            if (!productProfileValidationResult.IsSuccessful)
            {
                return productProfileValidationResult;
            }

            var settingTypesValidationResult = ValidateSettingTypes(settingCommand.Setting.SettingTypes);
            if (!settingTypesValidationResult.IsSuccessful)
            {
                return settingTypesValidationResult;
            }

            return Result.Success;
        }

        private IResult ValidateProductProfile(byte productId, IEnumerable<SettingType> customerSettingTypes)
        {
            var productSettingTypes = _productService.GetProductSettingTypes(productId);
            foreach (var settingType in customerSettingTypes)
            {
                if (!productSettingTypes.Where(stt => stt.Id == settingType.Id).Any())
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
                var settingTypeValidationResult = settingType.Validate();
                if (!settingTypeValidationResult.IsSuccessful)
                {
                    return settingTypeValidationResult;
                }
            }

            return Result.Success;
        }

        public IResult ValidateMultipleSettingCommand(MultipleSettingCommand multipleSettingCommand)
        {
            var settingCommandValidationResult = ValidateSettingCommand(multipleSettingCommand);
            if (!settingCommandValidationResult.IsSuccessful)
            {
                return settingCommandValidationResult;
            }

            var permissionMultipleValidationResult = ValidatePermissionMultiple(multipleSettingCommand);
            if (!permissionMultipleValidationResult.IsSuccessful)
            {
                return permissionMultipleValidationResult;
            }

            return Result.Success;
        }

        private IResult ValidatePermissionMultiple(MultipleSettingCommand multipleSettingCommand)
        {
            if (multipleSettingCommand.Setting.Customer.Level == CustomerLevel.Member)
            {
                IEnumerable<int> custIds = multipleSettingCommand.Customers.Select(cust => cust.Id);

                var permissionMultipleValidationResult = _validationDataAccess.ValidatePermissionMultiple(
                    new PermissionValidationMultipleCommand
                    {
                        CustId = _settingContext.Actor.Id,
                        TargetRoleId = (int)multipleSettingCommand.Setting.Customer.Level,
                        TargetCustIds = string.Join(",", custIds)
                    });

                return permissionMultipleValidationResult;
            }

            return Result.Success;
        }
    }
}