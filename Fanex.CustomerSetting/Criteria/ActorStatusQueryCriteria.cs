namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class ActorStatusQueryCriteria : CriteriaBase
    {
        public int ActorId { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_ActorStatusSelect";
        }

        public override bool IsValid()
        {
            return ActorId > 0;
        }
    }
}