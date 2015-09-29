namespace Fanex.CustomerSetting.SettingType1.Models
{
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType1.Constants;
    using Fanex.CustomerSetting.SettingType1.Resources;

    public class AgentPTOnMemberItem : PTItem
    {
        private const string PositionTakingXmlFormat = "<PT {0} AgentPT=\"{1}\"/>";

        /// <summary>
        /// Agent PT
        /// </summary>
        public byte A1 { get; set; }

        /// <summary>
        /// Agent Max PT
        /// </summary>
        public byte A2 { get; set; }

        /// <summary>
        /// Agent Min PT
        /// </summary>
        public byte N2 { get; set; }

        public override string ToXml()
        {
            return string.Format(
                PositionTakingXmlFormat,
                base.ToXml(),
                A1);
        }
    }
}