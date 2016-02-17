namespace Fanex.CustomerSetting.Casino.Services.Impl
{
    using Common.Helper;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Models;

    /// <summary>
    /// <see cref="https://redmine.nexcel.vn/projects/allbet/wiki/Allbet_Credit_Changes_On_NAP#2.1"/>
    /// </summary>
    public class QueryCasinoSettingService : IQuerySettingService
    {
        protected readonly ISettingContext settingContext;
        protected readonly IQuerySettingService querySettingService;
        protected readonly ICustomerProductStatusService customerProductStatusService;

        public QueryCasinoSettingService(
            ISettingContext settingContext,
            IQuerySettingService querySettingService = null,
            ICustomerProductStatusService customerProductStatusService = null)

        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");

            this.settingContext = settingContext;
            this.querySettingService = querySettingService ?? new QuerySettingService(settingContext);
            this.customerProductStatusService = customerProductStatusService ?? new CustomerProductStatusService();
        }

        public QueryCasinoSettingService(ISettingContext settingContext) : this(settingContext, null, null)
        {
        }

        protected QueryCasinoSettingService()
        {
        }

        public ISetting GetCustomerSetting(CustomerSettingQuery query)
        {
            var setting = querySettingService.GetCustomerSetting(query);

            var customerCasinoStatus = customerProductStatusService.GetCustomerStatus(query.CustomerId, query.ProductId);

            var casinoSetting = new CasinoSetting(setting, customerCasinoStatus);

            return casinoSetting;
        }
    }
}
