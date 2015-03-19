namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// AgentQueryMemberResult class.
    /// </summary>
    public class AgentQueryMemberResult
        : QueryResult<AgentQueryMemberCustomerSettingEntity,
            AgentQueryMemberDiscountEntity,
            AgentQueryMemberBetLimitEntity,
            AgentQueryMemberPositionTakingEntity,
            AgentQueryMemberProductStatusEntity>
    {
        public AgentQueryMemberResult(int errCode, string errMsg, SettingContext settingContext)
            : base(errCode, errMsg, settingContext)
        {
        }

        public AgentQueryMemberResult(SettingContext settingContext, AgentQueryMemberCustomerSettingEntity customerSetting)
            : base(settingContext, customerSetting)
        {
        }
    }
}