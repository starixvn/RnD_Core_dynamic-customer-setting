namespace Fanex.CustomerSetting.Entities
{
    /// <summary>
    /// MasterEditAgentCustomerSetting class.
    /// </summary>
    public class MasterQueryAgentCustomerSettingEntity
        : CustomerSettingEntity<MasterQueryAgentDiscountEntity,
            MasterQueryAgentBetLimitEntity,
            MasterQueryAgentPositionTakingEntity,
            MasterQueryAgentProductStatusEntity>
    {
    }
}