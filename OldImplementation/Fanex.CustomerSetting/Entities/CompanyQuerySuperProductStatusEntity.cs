namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class CompanyQuerySuperProductStatusEntity : ProductStatusEntity
    {
        [OutputElement]
        public bool IsDisabledDownline { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && !(IsDisabled && !IsDisabledDownline);
        }
    }
}