namespace Fanex.CustomerSetting.SettingType1.Models
{
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType1.Constants;
    using Fanex.CustomerSetting.SettingType1.Resources;

    public class AgentPTItem : PTItem
    {
        private const string PTXmlFormat = "<PT {0} MasterPT=\"{1}\" IsAutoMasterPT=\"{2}\" AutoMasterPT=\"{3}\" AgentPT=\"{4}\" AgentMinPT=\"{5}\" UpdAllMems=\"{6}\" NewMasterPT=\"{7}\"/>";

        /// <summary>
        /// Master PT
        /// </summary>
        public byte M2 { get; set; }

        /// <summary>
        /// Master Auto PT
        /// </summary>
        public byte UM2 { get; set; }

        /// <summary>
        /// Is Auto
        /// </summary>
        public bool IM2 { get; set; }

        /// <summary>
        /// Agent Max PT
        /// </summary>
        public byte A2 { get; set; }

        /// <summary>
        /// Agent Min PT
        /// </summary>
        public byte N2 { get; set; }

        /// <summary>
        /// Master Max PT
        /// </summary>
        public byte M3 { get; set; }

        /// <summary>
        /// Master Min PT
        /// </summary>
        public byte N3 { get; set; }

        /// <summary>
        /// Agent Max PT
        /// </summary>
        public byte A4 { get; set; }

        public bool IsUpdateMasterPTOnMembers { get; set; }

        public byte MasterPTOnMembers { get; set; }

        public override IResult Validate()
        {
            if (M2 > Constants.MaxPTValue
                || UM2 > Constants.MaxPTValue
                || A2 > Constants.MaxPTValue
                || N2 > Constants.MaxPTValue)
            {
                return new Result(
                    ErrorCode.PTExceedMaxValue,
                    Message.PositionTakingCannotBeExceedMaxValue);
            }

            if (IM2 && M2 > 0)
            {
                return new Result(
                    ErrorCode.MasterPTIsGreaterThanZeroWhenIsAutoIsSet,
                    Message.MasterPTCannotBeGreaterThanZeroWhenIsAutoIsSet);
            }

            if (!IM2 && UM2 > 0)
            {
                return new Result(
                    ErrorCode.MasterAutoPTIsGreaterThanZeroWhenIsAutoIsNotSet,
                    Message.MasterAutoPTCannotBeGreaterThanZeroWhenIsAutoIsNotSet);
            }

            if (N2 > A2)
            {
                return new Result(
                    ErrorCode.AgentMinPTIsGreaterThanAgentMaxPT,
                    Message.AgentMinPTCannotBeGreaterThanAgentMaxPT);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                PTXmlFormat,
                base.ToXml(),
                M2,
                IM2 ? 1 : 0,
                UM2,
                A2,
                N2,
                IsUpdateMasterPTOnMembers ? 1 : 0,
                MasterPTOnMembers);
        }
    }
}