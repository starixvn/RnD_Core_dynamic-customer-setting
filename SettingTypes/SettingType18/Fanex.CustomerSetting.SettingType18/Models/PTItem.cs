namespace Fanex.CustomerSetting.SettingType18.Models
{
    using CustomerSetting.Models;

    public class PTItem : SettingItemDefault
    {
        private const string BasePTXmlFormat = "{0} IsLive=\"{1}\"";

        public bool IsLive { get; set; }

        public bool IsUpdateMinPtOnly { get; set; }

        public override string ToXml()
        {
            return string.Format(BasePTXmlFormat, base.ToXml(), IsLive ? 1 : 0);
        }
    }
}