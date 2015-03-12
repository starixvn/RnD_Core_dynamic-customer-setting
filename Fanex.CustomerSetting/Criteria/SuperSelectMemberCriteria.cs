namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// AgentSettingSelectCriteria class.
    /// </summary>
    public class SuperSelectMemberCriteria : SelectCriteria
    {
        public SuperSelectMemberCriteria(int custId, short productId)
            : base(custId, productId)
        {
        }

        public int RoleId
        {
            get
            {
                return (int)RoleType.Super;
            }
        }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_MemberSettingSelect";
        }
    }
}