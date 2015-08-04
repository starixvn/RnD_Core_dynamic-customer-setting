namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    internal class CheckBelongingCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "CustId")]
        public int ActorId { get; set; }

        [SpParam(SqlParamName = "TargetCustId")]
        public int CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_SameLineCheck";
        }

        public override bool IsValid()
        {
            return ActorId > 0 && CustomerId > 0;
        }
    }
}