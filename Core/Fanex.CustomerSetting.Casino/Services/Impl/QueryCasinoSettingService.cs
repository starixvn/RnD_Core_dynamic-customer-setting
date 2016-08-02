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
        private readonly ISettingContext _settingContext;
        private readonly IQuerySettingService _querySettingService;
        private readonly ICustomerProductStatusService _customerProductStatusService;

        public QueryCasinoSettingService(
            ISettingContext settingContext,
            IQuerySettingService querySettingService = null,
            ICustomerProductStatusService customerProductStatusService = null)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(settingContext.Actor, nameof(settingContext.Actor));

            _settingContext = settingContext;
            _querySettingService = querySettingService ?? new QuerySettingService(_settingContext);
            _customerProductStatusService = customerProductStatusService ?? new CustomerProductStatusService();
        }

        public QueryCasinoSettingService(ISettingContext settingContext) : this(settingContext, null, null)
        {
        }

        protected QueryCasinoSettingService()
        {
        }

        public ISetting GetCustomerSetting(CustomerSettingQuery query)
        {
            Check.Argument.IsNotNull(query, nameof(query));

            var setting = _querySettingService.GetCustomerSetting(query);
            var customerCasinoStatus = _customerProductStatusService.GetCustomerStatus(query.CustomerId, query.ProductId);
            var casinoSetting = new CasinoSetting(setting, customerCasinoStatus);

            return casinoSetting;
        }
    }
}