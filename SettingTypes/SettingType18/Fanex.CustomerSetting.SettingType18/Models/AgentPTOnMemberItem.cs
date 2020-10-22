namespace Fanex.CustomerSetting.SettingType18.Models
{
    public class AgentPTOnMemberItem : PTItem
    {
        private const string PositionTakingXmlFormat = "<PT {0} AgentPT=\"{1}\"/>";

        /// <summary>
        /// Agent PT
        /// </summary>
        public decimal A1 { get; set; }

        /// <summary>
        /// Agent Max PT
        /// </summary>
        public decimal A2 { get; set; }

        /// <summary>
        /// Agent Min PT
        /// </summary>
        public decimal N2 { get; set; }

        public override string ToXml()
        {
            return string.Format(
                PositionTakingXmlFormat,
                base.ToXml(),
                A1);
        }
    }
}