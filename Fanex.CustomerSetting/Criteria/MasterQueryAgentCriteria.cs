namespace Fanex.CustomerSetting.Criteria
{
    public class MasterQueryAgentCriteria : QueryCriteria
    {
        public MasterQueryAgentCriteria(int custId, short productId)
            : base(custId, productId)
        {
        }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_AgentSettingSelect";
        }
    }
}