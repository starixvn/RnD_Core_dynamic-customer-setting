namespace Fanex.CustomerSetting.NumberGames2.Services.Impl
{
    using System;
    using System.Linq;
    using Common.Helper;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Logging;

    public class UpdateNumberGamesSettingService : UpdateSettingService, IUpdateSettingService
    {
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

        public override IResult Update(UpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

            throw new NotImplementedException();
        }

        public override IResult Update(MultipleUpdateCustomerSettingCommand command)
        {
            throw new NotImplementedException();
        }
    }
}