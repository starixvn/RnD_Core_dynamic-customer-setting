namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;
    using Fanex.CustomerSetting.SettingType1.Template1.Resources;

    public class SuperPTItem : PTItem
    {
        private const string PTXmlFormat = "<PT {0} SuperPT=\"{1}\" MasterPT=\"{2}\" AgentPT=\"{3}\" SuperMinPT=\"{4}\"/>";

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
            if (S4 > Constants.MaxPTValue
                || M4 > Constants.MaxPTValue
                || A4 > Constants.MaxPTValue
                || N4 > Constants.MaxPTValue)
            {
                return new Result(
                    ErrorCode.PTExceedMaxValue,
                    PTMessage.PositionTakingCannotBeExceedMaxValue);
            }

            if (M4 > S4)
            {
                return new Result(
                    ErrorCode.MasterMaxPTIsGreaterThanSuperMaxPT,
                    PTMessage.MasterMaxPTCannotBeGreaterThanSuperMaxPT);
            }

            if (A4 > M4)
            {
                return new Result(
                    ErrorCode.AgentMaxPTIsGreaterThanMasterMaxPT,
                    PTMessage.AgentMaxPTCannotBeGreaterThanMasterMaxPT);
            }

            if (N4 > S4)
            {
                return new Result(
                    ErrorCode.SuperMinPTIsGreaterThanSuperMaxPTErrorCode,
                    PTMessage.SuperMinPTCannotBeGreaterThanSuperMaxPT);
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