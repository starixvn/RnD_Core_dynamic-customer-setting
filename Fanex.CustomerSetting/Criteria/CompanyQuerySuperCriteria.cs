namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// CompanyQuerySuperCriteria class.
    /// </summary>
    public class CompanyQuerySuperCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public byte ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_SuperSettingSelect";
        }

        public override bool IsValid()
        {
            return CustId > 0 && ProductId > 0;
        }
    }
}