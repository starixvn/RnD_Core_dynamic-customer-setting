namespace Fanex.CustomerSetting.SettingType1.Models
{
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType1.Constants;
    using Fanex.CustomerSetting.SettingType1.Resources;

    public class SuperPTItem : PTItem
    {
        private const string PTXmlFormat = "<PT {0} SuperPT=\"{1}\" MasterPT=\"{2}\" AgentPT=\"{3}\" MinPT=\"{4}\"/>";

        /// <summary>
        /// Super Max PT
        /// </summary>
        public byte S4 { get; set; }

        /// <summary>
        /// Master Max PT
        /// </summary>
        public byte M4 { get; set; }

        /// <summary>
        /// Agent Max PT
        /// </summary>
        public byte A4 { get; set; }

        /// <summary>
        /// Super Min PT
        /// </summary>
        public byte N4 { get; set; }

        public byte DefaultMaxPT { get; set; }

        public override IResult Validate()
        {
            if (M4 > S4)
            {
                return new Result(
                    ErrorCode.MasterMaxPTIsGreaterThanSuperMaxPT,
                    Message.MasterMaxPTCannotBeGreaterThanSuperMaxPT);
            }

            if (A4 > M4)
            {
                return new Result(
                    ErrorCode.AgentMaxPTIsGreaterThanMasterMaxPT,
                    Message.AgentMaxPTCannotBeGreaterThanMasterMaxPT);
            }

            if (N4 > S4)
            {
                return new Result(
                    ErrorCode.SuperMinPTIsGreaterThanSuperMaxPTErrorCode,
                    Message.SuperMinPTCannotBeGreaterThanSuperMaxPT);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                PTXmlFormat,
                base.ToXml(),
                S4,
                M4,
                A4,
                N4);
        }
    }
}