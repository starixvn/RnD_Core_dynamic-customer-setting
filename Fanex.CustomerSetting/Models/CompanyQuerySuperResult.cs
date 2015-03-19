namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// CompanyQuerySuperResult class.
    /// </summary>
    public class CompanyQuerySuperResult
        : QueryResult<CompanyQuerySuperCustomerSettingEntity,
            CompanyQuerySuperDiscountEntity,
            CompanyQuerySuperBetLimitEntity,
            CompanyQuerySuperPositionTakingEntity,
            CompanyQuerySuperProductStatusEntity>
    {
        public CompanyQuerySuperResult(int errCode, string errMsg, SettingContext settingContext)
            : base(errCode, errMsg, settingContext)
        {
        }

        public CompanyQuerySuperResult(SettingContext settingContext, CompanyQuerySuperCustomerSettingEntity customerSetting)
            : base(settingContext, customerSetting)
        {
        }
    }
}