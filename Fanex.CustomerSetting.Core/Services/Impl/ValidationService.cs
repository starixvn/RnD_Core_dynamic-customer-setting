namespace Fanex.CustomerSetting.Core.Services.Impl
{
    using System;
    using Fanex.CustomerSetting.Core.Models.Params;
    using Fanex.CustomerSetting.Core.Results;

    public class ValidationService : IValidationService
    {
        public IResult ValidateSettingQuery(SettingQuery settingQuery)
        {
            throw new NotImplementedException();
        }

        public IResults ValidateSettingCommand(SettingCommand settingCommand)
        {
            throw new NotImplementedException();
        }

        public IMultipleResults ValidateMultipleSettingCommand(MultipleSettingCommand multipleSettingCommand)
        {
            throw new NotImplementedException();
        }
    }
}