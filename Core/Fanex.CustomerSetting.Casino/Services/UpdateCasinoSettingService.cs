namespace Fanex.CustomerSetting.Casino.Services
{
    using System;
    using Models;
    using Models.Requests;
    using Fanex.CustomerSetting.Services;
    using Models.Results;

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
