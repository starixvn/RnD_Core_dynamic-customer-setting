namespace Fanex.CustomerSetting.Criteria
{
    using Data.Repository;

    internal class ProductNamesQueryCriteria : CriteriaBase
    {
        public byte? ProductId { get; set; }

        public string LanguageKey { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_ProductNamesSelect";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}