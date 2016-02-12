namespace Fanex.CustomerSetting.Casino.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Common.Helper;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Services;
    using Logging;
    public class UpdateCasinoSettingService : UpdateSettingService
    {
        public UpdateCasinoSettingService()
        {

        }

        public UpdateCasinoSettingService(ISettingContext settingContext)
            : this(settingContext, null, null, null, null)
        {
        }

        public UpdateCasinoSettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            ITaskService pluggableTaskService = null,
            ILogger logger = null,
            ISettingTypeServiceFactory settingTypeServiceFactory = null)
            : base(settingContext, validationService, pluggableTaskService, logger, settingTypeServiceFactory) 
        {
            
        }

        protected override IEnumerable<SettingResult> UpdateCustomerSettingTypes(UpdateCustomerSettingCommand command)
        {
            return base.UpdateCustomerSettingTypes(command);
        }

        public override IResult Update(MultipleUpdateCustomerSettingCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
