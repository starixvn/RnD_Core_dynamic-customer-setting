namespace Fanex.CustomerSetting.Criteria.Product
{
    using Fanex.NAP.Framework.Data;

    public class ProductNamesQueryCriteria : CriteriaBase
    {
        public byte? ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_ProductNamesSelect";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}