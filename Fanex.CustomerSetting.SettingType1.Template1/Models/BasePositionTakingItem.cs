namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Models;

    public class BasePositionTakingItem : DefaultSettingItem
    {
        private const string BasePositionTakingXmlFormat = "{0} IsLive=\"{1}\"";

        public bool IsLive { get; set; }

        public override string ToXml()
        {
            return string.Format(BasePositionTakingXmlFormat, base.ToXml(), IsLive ? 1 : 0);
        }
    }
}