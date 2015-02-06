namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// SuperSelectMasterResult class.
    /// </summary>
    public class SuperSelectMasterResult
        : SelectResult<SuperSelectMasterCustomerSettingEntity,
            SuperSelectMasterDiscountEntity,
            SuperSelectMasterBetLimitEntity,
            SuperSelectMasterPositionTakingEntity,
            SuperSelectMasterProductStatusEntity>
    {
        public SuperSelectMasterResult(int errCode, string errMsg, SettingContext settingContext)
            : base(errCode, errMsg, settingContext)
        {
        }

        public SuperSelectMasterResult(SettingContext settingContext, SuperSelectMasterCustomerSettingEntity customerSetting)
            : base(settingContext, customerSetting)
        {
        }
    }
}