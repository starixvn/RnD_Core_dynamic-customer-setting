namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// SuperSelectMasterResult class.
    /// </summary>
    public class MasterSelectAgentResult
        : SelectResult<MasterSelectAgentCustomerSettingEntity,
            MasterSelectAgentDiscountEntity,
            MasterSelectAgentBetLimitEntity,
            MasterSelectAgentPositionTakingEntity,
            MasterSelectAgentProductStatusEntity>
    {
        public MasterSelectAgentResult(int errCode, string errMsg, SettingContext settingContext)
            : base(errCode, errMsg, settingContext)
        {
        }

        public MasterSelectAgentResult(SettingContext settingContext, MasterSelectAgentCustomerSettingEntity customerSetting)
            : base(settingContext, customerSetting)
        {
        }
    }
}