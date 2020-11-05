namespace Fanex.CustomerSetting.SettingType18.Models
{
    public class SuperPTOnMemberItem : PTItem
    {
        private const string PTXmlFormat = "<PT {0} SuperPT=\"{1}\" AutoSuperPT=\"{2}\" IsAutoSuperPT=\"{3}\"/>";

        /// <summary>
        /// Agent PT on Member
        /// </summary>
        public decimal A1 { get; set; }

        /// <summary>
        /// Master PT On member
        /// </summary>
        public decimal M1 { get; set; }

        /// <summary>
        /// Super PT On Member
        /// </summary>
        public decimal S1 { get; set; }

        /// <summary>
        /// Super auto PT On Member
        /// </summary>
        public decimal US1 { get; set; }

        /// <summary>
        /// Is Super Auto on Member.
        /// </summary>
        public bool IS1 { get; set; }

        /// <summary>
        /// Is Auto on Master.
        /// </summary>
        public bool IS3 { get; set; }

        /// <summary>
        /// Super Max PT
        /// </summary>
        public decimal S4 { get; set; }

        /// <summary>
        /// Super Min PT
        /// </summary>
        public decimal N4 { get; set; }

        /// <summary>
        /// Master Max PT
        /// </summary>
        public decimal M3 { get; set; }

        public override string ToXml()
        {
            return string.Format(
                PTXmlFormat,
                base.ToXml(),
                IS1 ? 0 : S1,
                IS1 ? US1 : 0,
                IS1 ? 1 : 0);
        }
    }
}