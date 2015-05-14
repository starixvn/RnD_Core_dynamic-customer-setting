namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class MasterQueryAgentProductStatusEntity : ProductStatusEntity
    {
        [OutputElement]
        public bool IsDisabledDownline { get; set; }

        public bool IsDisabledUpline { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && !(IsDisabled && !IsDisabledDownline);
        }
    }
}