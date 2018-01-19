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
    public class QueryCasinoSettingService : QuerySettingService
    {
        private readonly ICustomerProductSyncedStatusService _customerProductSyncedStatusService;

        public QueryCasinoSettingService(
            ISettingContext settingContext,
            ITaskService pluggableTaskService,
            ICustomerProductSyncedStatusService customerProductStatusService)
            : base(settingContext, pluggableTaskService: pluggableTaskService)
        {
            _customerProductSyncedStatusService = customerProductStatusService ?? new CustomerProductSyncedStatusService();
        }

        public QueryCasinoSettingService(ISettingContext settingContext) : this(settingContext, null, null)
        {
        }

        public override ISetting GetCustomerSetting(CustomerSettingQuery query)
        {
            Check.Argument.IsNotNull(query, nameof(query));

            ISetting setting = base.GetCustomerSetting(query);
            bool syncedStatus = _customerProductSyncedStatusService.CheckCustomerProductSyncedStatus(query.CustomerId, query.ProductId);
            var casinoSetting = new CasinoSetting(setting, syncedStatus);

            return casinoSetting;
        }
    }
}