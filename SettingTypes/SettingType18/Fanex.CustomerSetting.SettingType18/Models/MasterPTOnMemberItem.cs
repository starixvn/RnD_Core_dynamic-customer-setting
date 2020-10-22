namespace Fanex.CustomerSetting.SettingType18.Models
{
    public class MasterPTOnMemberItem : PTItem
    {
        private const string PTXmlFormat = "<PT {0} MasterPT=\"{1}\" AutoMasterPT=\"{2}\" IsAutoMasterPT=\"{3}\"/>";

        /// <summary>
        /// Agent PT on Member
        /// </summary>
        public decimal A1 { get; set; }

        /// <summary>
        /// Master PT On member
        /// </summary>
        public decimal M1 { get; set; }

        /// <summary>
        /// Is Master auto on Member
        /// </summary>
        public bool IM1 { get; set; }

        /// <summary>
        /// Super Auto PT on Member
        /// </summary>
        public decimal UM1 { get; set; }

        /// <summary>
        /// Is master auto on Agent
        /// </summary>
        public bool IM2 { get; set; }

        /// <summary>
        /// Master Max PT
        /// </summary>
        public decimal M3 { get; set; }

        /// <summary>
        /// Master Min PT
        /// </summary>
        public decimal N3 { get; set; }

        public override string ToXml()
        {
            return string.Format(
                PTXmlFormat,
                base.ToXml(),
                IM1 ? 0 : M1,
                IM1 ? UM1 : 0,
                IM1 ? 1 : 0);
        }
    }
}