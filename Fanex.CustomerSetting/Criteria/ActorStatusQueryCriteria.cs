namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class ActorStatusQueryCriteria : CriteriaBase
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