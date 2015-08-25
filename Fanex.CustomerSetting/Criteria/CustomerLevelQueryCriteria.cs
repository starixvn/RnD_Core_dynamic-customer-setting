namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    internal class CustomerLevelQueryCriteria : CriteriaBase
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