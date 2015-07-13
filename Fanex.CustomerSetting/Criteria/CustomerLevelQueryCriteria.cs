namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class CustomerLevelQueryCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_CustomerLevelSelect";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}