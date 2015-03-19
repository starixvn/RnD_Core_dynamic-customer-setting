namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// SuperQueryMasterResult class.
    /// </summary>
    public class SuperQueryMasterResult
        : QueryResult<SuperQueryMasterCustomerSettingEntity,
            SuperQueryMasterDiscountEntity,
            SuperQueryMasterBetLimitEntity,
            SuperQueryMasterPositionTakingEntity,
            SuperQueryMasterProductStatusEntity>
    {
        public SuperQueryMasterResult(int errCode, string errMsg, SettingContext settingContext)
            : base(errCode, errMsg, settingContext)
        {
        }

        public SuperQueryMasterResult(SettingContext settingContext, SuperQueryMasterCustomerSettingEntity customerSetting)
            : base(settingContext, customerSetting)
        {
        }
    }
}