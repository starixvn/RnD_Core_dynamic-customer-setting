namespace Fanex.CustomerSetting.Core.SPObjects.Criteria.Product
{
    using Fanex.NAP.Framework.Data;

    public class ProductResourceQueryCriteria : CriteriaBase
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