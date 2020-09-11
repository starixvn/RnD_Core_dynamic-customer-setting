namespace Fanex.CustomerSetting.Casino.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Exceptions;
    using Logging;

    public class UpdateCasinoSettingService : UpdateSettingService, IUpdateCasinoSettingService
    {
        private const byte StatusSettingTypeValue = 13;
        private readonly ICustomerProductSyncedStatusService _customerProductSyncedStatusService;

        public UpdateCasinoSettingService(ISettingContext settingContext, ICustomerProductSyncedStatusService customerProductStatusService)
            : base(settingContext)
        {
            _customerProductSyncedStatusService = customerProductStatusService;
        }

        public UpdateCasinoSettingService(ISettingContext settingContext)
            : this(settingContext, new CustomerProductSyncedStatusService())
        {
        }

        internal UpdateCasinoSettingService(
            ISettingContext settingContext,
            IValidationService validationService,
            ITaskService pluggableTaskService,
            ILogger logger,
            ISettingTypeServiceFactory settingTypeServiceFactory,
            ICustomerProductSyncedStatusService customerProductStatusService)
            : base(settingContext, validationService, pluggableTaskService, logger, settingTypeServiceFactory)
        {
            _customerProductSyncedStatusService = customerProductStatusService;
        }

        public override IResult Update(UpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNull(command.Setting, nameof(command.Setting));

            bool syncedStatus = _customerProductSyncedStatusService.CheckCustomerProductSyncedStatus(
                command.Setting.CustomerId,
                command.Setting.ProductId);

            if (syncedStatus)
            {
                return base.Update(command);
            }

            return UpdateUnsyncedCustomerSetting(command);
        }

        public override IResult Update(MultipleUpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNull(command.Setting, nameof(command.Setting));
            Check.Argument.IsNotNull(command.Setting.SettingTypes, nameof(command.Setting.SettingTypes));
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

            var subResults = new List<IResult>();

            TaskService.Run(command.Setting.ProductId, command);

            foreach (var customerId in command.CustomerIds)
            {
                var updateCustomerSettingCommand = command.GetUpdateCustomerSettingCommand(customerId);
                var updateResult = Update(updateCustomerSettingCommand);

                subResults.Add(updateResult);
            }

            var result = new UpdateSettingResult(subResults);

            TaskService.Run(command.Setting.ProductId, command, result);

            return result;
        }

        public IResult UpdateUnsyncedCustomerSetting(UpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNull(command.Setting, nameof(command.Setting));
            Check.Argument.IsNotNull(command.Setting.SettingTypes, nameof(command.Setting.SettingTypes));

            var settingTypeResults = new List<SettingResult>();

            TaskService.Run(command.Setting.ProductId, command);

            command.Setting.SettingTypes
                .Where(settingType => settingType.Id != StatusSettingTypeValue)
                .ToList()
                .ForEach(settingType =>
                {
                    var updateSettingTypeResult = UpdateCustomerSettingType(
                        command.Setting.CustomerId,
                        command.Setting.CustomerInfo,
                        command.Setting.CustomerLevel,
                        settingType,
                        command.Setting.ProductId,
                        command.UplineLevel);

                    settingTypeResults.Add(updateSettingTypeResult);
                });

            if (settingTypeResults.All(settingTypeResult => settingTypeResult.IsOk) && SystemType.IsNotExternalNapBetSetting(SettingContext.System))
            {
                var productStatusResult = UpdateProductStatus(command);

                if (productStatusResult != null)
                {
                    settingTypeResults.Add(productStatusResult);
                }
            }

            var result = new UpdateSettingResult(settingTypeResults);

            TaskService.Run(command.Setting.ProductId, command, result);

            return result;
        }

        private SettingResult UpdateProductStatus(UpdateCustomerSettingCommand command)
        {
            SettingResult productStatusResult = null;
            var statusSettingType = command.Setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == StatusSettingTypeValue);

            if (statusSettingType != null)
            {
                productStatusResult = UpdateCustomerSettingType(
                    command.Setting.CustomerId,
                    command.Setting.CustomerInfo,
                    command.Setting.CustomerLevel,
                    statusSettingType,
                    command.Setting.ProductId,
                    command.UplineLevel);

                if (productStatusResult.IsOk)
                {
                    _customerProductSyncedStatusService.UpdateCustomerProductSyncedStatus(
                        command.Setting.CustomerId,
                        command.Setting.ProductId,
                        SettingContext.Actor.Id,
                        SettingContext.Actor.SubAccountId,
                        SettingContext.IP);
                }
            }
#pragma warning disable S126 // "if ... else if" constructs should end with "else" clauses
            else if (command.Setting.CustomerLevel.IsNotMemberLevel() || command.UplineLevel?.IsAgentLevel() == true)
#pragma warning restore S126 // "if ... else if" constructs should end with "else" clauses
            {
                throw new CustomerSettingException($"Missing Product Status of customer {command.Setting.CustomerId}.");
            }

            return productStatusResult;
        }
    }
}