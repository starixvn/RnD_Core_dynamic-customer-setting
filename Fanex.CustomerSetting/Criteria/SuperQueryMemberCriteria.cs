namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// SuperQueryMemberCriteria class.
    /// </summary>
    public class SuperQueryMemberCriteria : QueryCriteria
    {
        public SuperQueryMemberCriteria(int custId, short productId)
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