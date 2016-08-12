namespace Fanex.CustomerSetting.NumberGames2.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Exceptions;
    using Logging;
    using Models;
    using Resources;

    public class UpdateNumberGames2SettingService : UpdateSettingService, IUpdateSettingService
    {
        private const short ProductStatusSettingType = 11;
        ////private const short CommissionGroupSettingType = 10;

        private readonly ICustomerSyncStatusService _customerSyncStatusService;

        public UpdateNumberGames2SettingService(ISettingContext settingContext)
            : this(settingContext, null, null, null, null, null)
        {
        }

        public UpdateNumberGames2SettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            ITaskService pluggableTaskService = null,
            ILogger logger = null,
            ISettingTypeServiceFactory settingTypeServiceFactory = null,
            ICustomerSyncStatusService customerSyncStatusService = null)
            : base(settingContext, validationService, pluggableTaskService, logger, settingTypeServiceFactory)
        {
            _customerSyncStatusService = customerSyncStatusService ?? new CustomerSyncStatusService();
        }

        public override IResult Update(UpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

            ////CustomerSyncStatus syncStatus = _customerSyncStatusService.GetCustomerSyncStatus(
            ////    command.Setting.CustomerId,
            ////    command.Setting.ProductId);

            // Cheat here while waiting for SPs and APIs.
            var syncStatus = new CustomerSyncStatus
            {
                IsSynced = true,
                IsUplineSynced = true
            };

            if (!syncStatus.IsUplineSynced)
            {
                throw new CustomerSettingException(
                    $"Product {command.Setting.ProductId} has not been offered for Upline of Customer {command.Setting.CustomerId}.");
            }

            if (syncStatus.IsSynced)
            {
                SyncUserGroup(command);

                return base.Update(command);
            }

            var settingTypeResults = new List<SettingResult>();

            SyncCustomer(command, settingTypeResults);
            SyncUserGroup(command);
            UpdateCustomerSettingTypes(
                command.Setting.CustomerId,
                command.Setting.CustomerInfo,
                command.Setting.CustomerLevel,
                command.Setting.ProductId,
                command.UplineLevel,
                command.Setting.SettingTypes.Where(settingType => settingType.Id != ProductStatusSettingType),
                settingTypeResults);

            return new UpdateSettingResult(settingTypeResults);
        }

        public override IResult Update(MultipleUpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNull(command.Setting, nameof(command.Setting));
            Check.Argument.IsNotNull(command.Setting.SettingTypes, nameof(command.Setting.SettingTypes));
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

            var results = new List<IResult>();

            foreach (var customerId in command.CustomerIds)
            {
                var updateOneCustomerSettingCommand = command.GetUpdateCustomerSettingCommand(customerId);
                var result = Update(updateOneCustomerSettingCommand);

                results.Add(result);
            }

            return new UpdateSettingResult(results);
        }

        private void SyncCustomer(UpdateCustomerSettingCommand command, List<SettingResult> settingTypeResults)
        {
            // @@TODO: Call API to sync the customer profile.
            // @@TODO: Should we sync the user group along with customer profile at one step?
            bool syncResult = true;
            SettingType productStatusSettingType = command.Setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == ProductStatusSettingType);

            if (syncResult)
            {
                _customerSyncStatusService.UpdateCustomerSyncStatus(
                    command.Setting.CustomerId,
                    command.Setting.ProductId,
                    SettingContext.Actor.Id,
                    SettingContext.Actor.SubAccountId,
                    SettingContext.IP);

                if (productStatusSettingType != null)
                {
                    SettingResult updateProductStatusResult = UpdateCustomerSettingType(
                        command.Setting.CustomerId,
                        command.Setting.CustomerInfo,
                        command.Setting.CustomerLevel,
                        productStatusSettingType,
                        command.Setting.ProductId,
                        command.UplineLevel);

                    settingTypeResults.Add(updateProductStatusResult);
                }
            }
            else if (productStatusSettingType != null)
            {
                var updateProductStatusResult = new SettingResult(
                    ErrorCode.UnableToUpdateProductStatus,
                    Messages.UnableToUpdateProductStatus,
                    MessageType.ProductStatusViolation,
                    command.Setting.CustomerId,
                    productStatusSettingType.Id,
                    productStatusSettingType.Name);

                settingTypeResults.Add(updateProductStatusResult);
            }
        }

        private void SyncUserGroup(UpdateCustomerSettingCommand command)
        {
            if (command.Setting.CustomerLevel.IsMemberLevel())
            {
                // @@TODO: Check whether user edited Commission.
                // @@TODO: Check whether it is the first time edit
                // @@TODO: Check whether UserGroup in Commission is updated.
                bool allowGroupUpdate = true;
                ////string newUserGroup = "a";

                if (allowGroupUpdate)
                {
                    // @@TODO: Call API to sync member group.
                }
            }
        }

        private void UpdateCustomerSettingTypes(
            int customerId,
            CustomerInfo customerInfo,
            Level customerLevel,
            byte productId,
            Level uplineLevel,
            IEnumerable<SettingType> settingTypes,
            List<SettingResult> settingTypeResults)
        {
            foreach (SettingType settingType in settingTypes)
            {
                var updateSettingTypeResult = UpdateCustomerSettingType(
                               customerId,
                               customerInfo,
                               customerLevel,
                               settingType,
                               productId,
                               uplineLevel);

                settingTypeResults.Add(updateSettingTypeResult);
            }
        }
    }
}