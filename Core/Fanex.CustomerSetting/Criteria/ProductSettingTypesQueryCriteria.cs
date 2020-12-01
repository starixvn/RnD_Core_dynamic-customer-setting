namespace Fanex.CustomerSetting.Criteria
{
    using Data.Repository;

    internal class ProductSettingTypesQueryCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        public byte SystemId { get; set; }

        public byte SettingStatus { get; set; }

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