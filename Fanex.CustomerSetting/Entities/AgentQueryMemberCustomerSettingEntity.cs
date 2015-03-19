namespace Fanex.CustomerSetting.Entities
{
    /// <summary>
    /// AgentEditMemberCustomerSetting class.
    /// </summary>
    public class AgentQueryMemberCustomerSettingEntity
        : CustomerSettingEntity<AgentQueryMemberDiscountEntity,
            AgentQueryMemberBetLimitEntity,
            AgentQueryMemberPositionTakingEntity,
            AgentQueryMemberProductStatusEntity>
    {
    }
}