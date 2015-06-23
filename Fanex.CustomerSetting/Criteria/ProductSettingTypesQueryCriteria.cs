namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class ProductSettingTypesQueryCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_ProductProfileSelect";
        }

        public override bool IsValid()
        {
            return ProductId > 0;
        }
    }
}