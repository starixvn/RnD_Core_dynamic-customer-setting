namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    public class GroupQueryOnMultipleMemberCriteria : CriteriaBase
    {
        public GroupQueryOnMultipleMemberCriteria()
        {
        }

        [SpParam(SqlParamName = "CustIDList")]
        public string CustIds { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(CustIds);
        }
    }
}