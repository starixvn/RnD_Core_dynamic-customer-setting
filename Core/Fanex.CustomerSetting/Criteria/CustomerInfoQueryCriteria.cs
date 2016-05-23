namespace Fanex.CustomerSetting.Criteria
{
    using Data.Repository;

    internal class CustomerInfoQueryCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_CustomerInfoSelect";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}