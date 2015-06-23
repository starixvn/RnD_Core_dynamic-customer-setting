namespace Fanex.CustomerSetting.Criteria.Customer
{
    using Fanex.NAP.Framework.Data;

    public class CustomerQueryCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_SettingDetailSelect";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}