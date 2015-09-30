namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    internal class ProductSettingTypesQueryCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        public byte SystemId { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_ProductProfileSelect";
        }

        public override bool IsValid()
        {
            return ProductId > 0;
        }
    }
}