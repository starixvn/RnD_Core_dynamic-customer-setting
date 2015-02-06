namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// CompanySelectSuperResult class.
    /// </summary>
    public class CompanySelectSuperResult
        : SelectResult<CompanySelectSuperCustomerSettingEntity,
            CompanySelectSuperDiscountEntity,
            CompanySelectSuperBetLimitEntity,
            CompanySelectSuperPositionTakingEntity,
            CompanySelectSuperProductStatusEntity>
    {
        public CompanySelectSuperResult(int errCode, string errMsg, SettingContext settingContext)
            : base(errCode, errMsg, settingContext)
        {
        }

        public CompanySelectSuperResult(SettingContext settingContext, CompanySelectSuperCustomerSettingEntity customerSetting)
            : base(settingContext, customerSetting)
        {
        }
    }
}