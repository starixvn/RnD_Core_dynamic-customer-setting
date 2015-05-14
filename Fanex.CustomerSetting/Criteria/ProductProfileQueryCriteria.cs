namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    public class ProductProfileQueryCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_SettingDetailSelect";
        }

        public override bool IsValid()
        {
            return ProductId > 0;
        }
    }
}