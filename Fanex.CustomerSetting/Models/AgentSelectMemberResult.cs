namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// SuperSelectMasterResult class.
    /// </summary>
    public class AgentSelectMemberResult
        : SelectResult<AgentSelectMemberCustomerSettingEntity,
            AgentSelectMemberDiscountEntity,
            AgentSelectMemberBetLimitEntity,
            AgentSelectMemberPositionTakingEntity,
            AgentSelectMemberProductStatusEntity>
    {
        public AgentSelectMemberResult(int errCode, string errMsg, SettingContext settingContext)
            : base(errCode, errMsg, settingContext)
        {
        }

        public AgentSelectMemberResult(SettingContext settingContext, AgentSelectMemberCustomerSettingEntity customerSetting)
            : base(settingContext, customerSetting)
        {
        }
    }
}