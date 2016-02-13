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
    using System.Linq;
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
            const byte StatusSettingTypeValue = 1;
            var settingTypes = command.Setting.SettingTypes;
            var customerId = command.Setting.CustomerId;
            var customerLevel = command.Setting.CustomerLevel;
            var uplineLevel = command.UplineLevel;
            var productId = command.Setting.ProductId;
            var settingTypeResults = new List<SettingResult>();

            SettingResult updateSettingTypeResult = null;

            settingTypes
                .Where(settingType => settingType.Id != StatusSettingTypeValue)
                .ToList()
                .ForEach(settingType =>
                {
                    updateSettingTypeResult = UpdateCustomerSettingType(
                               customerId,
                               command.Setting.CustomerInfo,
                               customerLevel,
                               settingType,
                               productId,
                               uplineLevel);

                    settingTypeResults.Add(updateSettingTypeResult);
                });

            if (!settingTypeResults.Exists(result => result.IsNotOk))
            {
                settingTypeResults.Add(
                    UpdateCustomerSettingType(
                                   customerId,
                                   command.Setting.CustomerInfo,
                                   customerLevel,
                                   settingTypes.First(settingType => settingType.Id == StatusSettingTypeValue),
                                   productId,
                                   uplineLevel));
            }

            return settingTypeResults;
        }

        public override IResult Update(MultipleUpdateCustomerSettingCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
