namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class ProductNamesQueryCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        public string LanguageKey { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_ProductNamesSelect";
        }

        public override bool IsValid()
        {
            return ProductId > 0 && !string.IsNullOrEmpty(LanguageKey);
        }
    }
}