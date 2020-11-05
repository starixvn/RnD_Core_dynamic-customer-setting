namespace Fanex.CustomerSetting.SettingType18.Models
{
    using Constants;
    using CustomerSetting.Models.Results;
    using Resources;

    public class AgentPTItem : PTItem
    {
        private const string PTXmlFormat
            = "<PT {0} MasterPT=\"{1}\" IsAutoMasterPT=\"{2}\" AutoMasterPT=\"{3}\" AgentPT=\"{4}\" MinPT=\"{5}\" IsUpdAllMems=\"{6}\" UpdAllMems_Value=\"{7}\"/>";

        /// <summary>
        /// Master PT
        /// </summary>
        public decimal M2 { get; set; }

        /// <summary>
        /// Master Auto PT
        /// </summary>
        public decimal UM2 { get; set; }

        /// <summary>
        /// Is Auto
        /// </summary>
        public bool IM2 { get; set; }

        /// <summary>
        /// Agent Max PT
        /// </summary>
        public decimal A2 { get; set; }

        /// <summary>
        /// Agent Min PT
        /// </summary>
        public decimal N2 { get; set; }

        /// <summary>
        /// Master Max PT
        /// </summary>
        public decimal M3 { get; set; }

        /// <summary>
        /// Master Min PT
        /// </summary>
        public decimal N3 { get; set; }

        /// <summary>
        /// Agent Max PT
        /// </summary>
        public decimal A4 { get; set; }

        public bool IsUpdateMasterPTOnMembers { get; set; }

        public decimal MasterPTOnMembers { get; set; }

        public override IResult Validate()
        {
            if (N2 > A2)
            {
                return new Result(
                    ErrorCodes.AgentMinPTIsGreaterThanAgentMaxPT,
                    Message.AgentMinPTCannotBeGreaterThanAgentMaxPT);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                PTXmlFormat,
                base.ToXml(),
                IM2 ? 0 : M2,
                IM2 ? 1 : 0,
                IM2 ? UM2 : 0,
                A2,
                N2,
                IsUpdateMasterPTOnMembers ? 1 : 0,
                MasterPTOnMembers);
        }
    }
}