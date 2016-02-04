namespace Fanex.CustomerSetting.Casino.Services
{
    using System;
    using Models;
    using Models.Requests;
    using Fanex.CustomerSetting.Services;

    public class QueryCasinoSettingService : IQuerySettingService
    {
        public QueryCasinoSettingService(ISettingContext settingContext)
        {

        }

        public ISetting GetCustomerSetting(CustomerSettingQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
