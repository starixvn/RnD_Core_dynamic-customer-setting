namespace Fanex.CustomerSetting.Criteria
{
    /// <summary>
    /// AgentSettingSelectCriteria class.
    /// </summary>
    public class MasterSelectAgentCriteria : SelectCriteria
    {
        public MasterSelectAgentCriteria(int custId, short productId, string settingKey)
            : base(custId, productId, settingKey)
        {
        }
    }
}