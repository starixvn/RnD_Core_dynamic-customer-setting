namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// MasterQueryAgentResult class.
    /// </summary>
    public class MasterQueryAgentResult
        : QueryResult<MasterQueryAgentCustomerSettingEntity,
            MasterQueryAgentDiscountEntity,
            MasterQueryAgentBetLimitEntity,
            MasterQueryAgentPositionTakingEntity,
            MasterQueryAgentProductStatusEntity>
    {
        public MasterQueryAgentResult(int errCode, string errMsg, SettingContext settingContext)
            : base(errCode, errMsg, settingContext)
        {
        }

        public MasterQueryAgentResult(SettingContext settingContext, MasterQueryAgentCustomerSettingEntity customerSetting)
            : base(settingContext, customerSetting)
        {
        }
    }
}