namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    internal class ActorStatusQueryCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_ActorStatusSelect";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}