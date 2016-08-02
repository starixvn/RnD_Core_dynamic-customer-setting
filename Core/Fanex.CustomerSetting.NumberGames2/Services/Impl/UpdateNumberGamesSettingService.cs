namespace Fanex.CustomerSetting.NumberGames2.Services.Impl
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Exceptions;
    using Logging;
    using Models;
    using Resources;

    public class UpdateNumberGamesSettingService : UpdateSettingService, IUpdateSettingService
    {
        private const short ProductStatusSettingType = 11;
        private const short CommissionGroupSettingType = 10;

        private readonly ICustomerSyncStatusService _customerSyncStatusService;

        public UpdateNumberGamesSettingService(ISettingContext settingContext)
            : this(settingContext, null, null, null, null, null)
        {
        }

        public UpdateNumberGamesSettingService(
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

        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed.")]
        public override IResult Update(UpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

            CustomerSyncStatus syncStatus = _customerSyncStatusService.GetCustomerSyncStatus(
                command.Setting.CustomerId,
                command.Setting.ProductId);

            if (!syncStatus.IsUplineSynced)
            {
                throw new CustomerSettingException(
                    $"Product {command.Setting.ProductId} has not been offered for Upline of Customer {command.Setting.CustomerId}.");
            }

            if (syncStatus.IsSynced)
            {
                return base.Update(command);
            }

            // @@TODO: Call API to sync the customer profile.
            bool syncResult = true;
            var settingTypeResults = new List<SettingResult>();
            SettingType productStatusSettingType = command.Setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == ProductStatusSettingType);

            if (syncResult)
            {
                // @@TODO: Update Sync Status.
                if (productStatusSettingType != null)
                {
                    var productStatusResult = UpdateCustomerSettingType(
                        command.Setting.CustomerId,
                        command.Setting.CustomerInfo,
                        command.Setting.CustomerLevel,
                        productStatusSettingType,
                        command.Setting.ProductId,
                        command.UplineLevel);

                    if (productStatusResult != null)
                    {
                        settingTypeResults.Add(productStatusResult);
                    }
                }
            }
            else if (productStatusSettingType != null)
            {
                settingTypeResults.Add(new SettingResult(
                    ErrorCode.UnableToUpdateProductStatus,
                    Messages.UnableToUpdateProductStatus,
                    MessageType.ProductStatusViolation,
                    command.Setting.CustomerId,
                    productStatusSettingType.Id,
                    productStatusSettingType.Name));
            }

            if (command.Setting.CustomerLevel.IsMemberLevel())
            {
                // @@TODO: Get data from SettingType: Commission.
                bool allowGroupUpdate = true;

                if (allowGroupUpdate)
                {
                    // @@TODO: Call API to sync member group.
                }
            }

            foreach (SettingType settingType in command.Setting.SettingTypes)
            {
                if (settingType.Id != ProductStatusSettingType)
                {
                    var updateSettingTypeResult = UpdateCustomerSettingType(
                               command.Setting.CustomerId,
                               command.Setting.CustomerInfo,
                               command.Setting.CustomerLevel,
                               settingType,
                               command.Setting.ProductId,
                               command.UplineLevel);

                    settingTypeResults.Add(updateSettingTypeResult);
                }
            }

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
                var updateCustomerSettingCommand = command.GetUpdateCustomerSettingCommand(customerId);
                var result = Update(updateCustomerSettingCommand);

                results.Add(result);
            }

            return new UpdateSettingResult(results);
        }
    }
}