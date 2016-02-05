namespace Fanex.CustomerSetting.Casino.Services.Impl
{
    using System;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Services;
    public class UpdateCasinoSettingService : IUpdateSettingService
    {
        public UpdateCasinoSettingService(ISettingContext settingContext)
        {

        }

        public IResult Update(MultipleUpdateCustomerSettingCommand command)
        {
            throw new NotImplementedException();
        }

        public IResult Update(UpdateCustomerSettingCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
