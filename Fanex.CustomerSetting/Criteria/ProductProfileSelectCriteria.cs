namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// ProductProfileSelectCriteria class.
    /// </summary>
    public class ProductProfileSelectCriteria : CriteriaBase
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