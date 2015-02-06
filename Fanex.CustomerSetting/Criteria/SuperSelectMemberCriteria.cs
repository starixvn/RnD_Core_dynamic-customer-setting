namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// AgentSettingSelectCriteria class.
    /// </summary>
    public class SuperSelectMemberCriteria : SelectCriteria
    {
        public SuperSelectMemberCriteria(int custId, short productId, string settingKey)
            : base(custId, productId, settingKey)
        {
        }

        public int RoleId
        {
            get
            {
                return (int)RoleType.Super;
            }
        }
    }
}