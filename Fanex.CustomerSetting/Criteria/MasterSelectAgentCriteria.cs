namespace Fanex.CustomerSetting.Criteria
{
    /// <summary>
    /// AgentSettingSelectCriteria class.
    /// </summary>
    public class MasterSelectAgentCriteria : SelectCriteria
    {
        public MasterSelectAgentCriteria(int custId, short productId)
            : base(custId, productId)
        {
        }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_AgentSettingSelect";
        }
    }
}