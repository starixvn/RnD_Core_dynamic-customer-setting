namespace Fanex.CustomerSetting.Casino.Services.Impl
{
    using Common.Helper;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Models;

    /// <summary>
    /// <see url="https://redmine.nexcel.vn/projects/allbet/wiki/Allbet_Credit_Changes_On_NAP#2.1"/>
    /// </summary>
    public class QueryCasinoSettingService : IQuerySettingService
    {
        protected readonly ISettingContext SettingContext;
        protected readonly IQuerySettingService QuerySettingService;
        protected readonly ICustomerProductStatusService CustomerProductStatusService;

        public QueryCasinoSettingService(
            ISettingContext settingContext,
            IQuerySettingService querySettingService = null,
            ICustomerProductStatusService customerProductStatusService = null)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");

            this.SettingContext = settingContext;
            this.QuerySettingService = querySettingService ?? new QuerySettingService(settingContext);
            this.CustomerProductStatusService = customerProductStatusService ?? new CustomerProductStatusService();
        }

        public QueryCasinoSettingService(ISettingContext settingContext) : this(settingContext, null, null)
        {
        }

        protected QueryCasinoSettingService()
        {
        }

        public ISetting GetCustomerSetting(CustomerSettingQuery query)
        {
            var setting = QuerySettingService.GetCustomerSetting(query);

            var customerCasinoStatus = CustomerProductStatusService.GetCustomerStatus(query.CustomerId, query.ProductId);

            var casinoSetting = new CasinoSetting(setting, customerCasinoStatus);

            return casinoSetting;
        }
    }
}