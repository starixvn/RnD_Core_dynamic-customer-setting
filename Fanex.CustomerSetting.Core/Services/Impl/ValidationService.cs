namespace Fanex.CustomerSetting.Core.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.DataAccesses.Impl;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Handlers;
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
            _settingContext = settingContext ?? SettingContextManager.SettingContext;
            _validationDataAccess = validationDataAccess ?? new ValidationDataAccess();
            _productService = productService ?? new ProductService();
        }

        public IResult ValidateSettingQuery(SettingQuery settingQuery)
        {
            Check.Argument.IsNotNull(settingQuery, "settingQuery");
            Check.Argument.IsNotNegativeOrZero(settingQuery.CustomerId, "settingQuery.CustomerId");
            Check.Argument.IsNotNegativeOrZero((byte)settingQuery.CustomerLevel, "settingQuery.CustomerLevel");

            return ValidateViewingPermission(settingQuery.CustomerId, settingQuery.CustomerLevel);
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
            return _validationDataAccess.ValidatePermission(new PermissionValidationCommand
            {
                ActorId = _settingContext.Actor.Id,
                CustomerId = customerId,
                CustomerLevelId = (int)customerLevel
            });
        }

        public IResult ValidateSettingCommand(SettingCommand settingCommand)
        {
            CheckValidateSettingCommandArguments(settingCommand);

            var setting = settingCommand.Setting;
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

        private void CheckValidateSettingCommandArguments(SettingCommand settingCommand)
        {
            Check.Argument.IsNotNull(settingCommand, "settingCommand");
            Check.Argument.IsNotNull(settingCommand.Setting, "settingCommand.Setting");
            Check.Argument.IsNotNull(settingCommand.Setting.Customer, "settingCommand.Setting.Customer");
            Check.Argument.IsNotNegativeOrZero(settingCommand.Setting.Customer.Id, "settingCommand.Setting.Customer.Id");
            Check.Argument.IsNotNull(settingCommand.Setting.SettingTypes, "settingCommand.Setting.SettingTypes");
        }

        private IResult ValidateProductProfile(byte productId, IEnumerable<SettingType> customerSettingTypes)
        {
            var productSettingTypes = _productService.GetProductSettingTypes(productId);
            foreach (var customerSettingType in customerSettingTypes)
            {
                if (!productSettingTypes.Where(stt => stt.Id == customerSettingType.Id).Any())
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

        public IResult ValidateMultipleSettingCommand(MultipleSettingCommand multipleSettingCommand)
        {
            CheckMultipleSettingCommandArguments(multipleSettingCommand);

            var commandValidationResult = ValidateSettingCommand(multipleSettingCommand);
            if (!commandValidationResult.IsSuccessful)
            {
                return commandValidationResult;
            }

            var permissionValidationResult = ValidateActorPermissionOnMultipleCustomers(
                multipleSettingCommand.Customers,
                multipleSettingCommand.Setting.Customer.Level);
            if (!permissionValidationResult.IsSuccessful)
            {
                return permissionValidationResult;
            }

            return Result.Success;
        }

        private void CheckMultipleSettingCommandArguments(MultipleSettingCommand multipleSettingCommand)
        {
            Check.Argument.IsNotNull(multipleSettingCommand, "settingCommand");
            Check.Argument.IsNotNull(multipleSettingCommand.Setting, "settingCommand.Setting");
            Check.Argument.IsNotNull(multipleSettingCommand.Customers, "multipleSettingCommand.Customers");
        }

        private IResult ValidateActorPermissionOnMultipleCustomers(IEnumerable<Customer> customers, CustomerLevel customerLevel)
        {
            if (customerLevel == CustomerLevel.Member)
            {
                IEnumerable<int> memberIds = customers.Select(cust => cust.Id);

                var permissionValidationResult = _validationDataAccess.ValidatePermissionMultiple(
                    new PermissionValidationMultipleCommand
                    {
                        ActorId = _settingContext.Actor.Id,
                        MemberIds = string.Join(",", memberIds)
                    });

                return permissionValidationResult;
            }

            return Result.Success;
        }
    }
}