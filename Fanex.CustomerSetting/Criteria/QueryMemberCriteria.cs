namespace Fanex.CustomerSetting.Criteria
{
    public class QueryMemberCriteria : QueryCriteria
    {
        public QueryMemberCriteria(int custId, int roleId, short productId)
            : base(custId, productId)
        {
            RoleId = roleId;
        }

        public int RoleId { get; private set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_MemberSettingSelect";
        }
    }
}