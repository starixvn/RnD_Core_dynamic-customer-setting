namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;

    public class SuperPositionTakingItem : BasePositionTakingItem
    {
        private const string PositionTakingXmlFormat = "<PT {0} SuperPT=\"{1}\" MasterPT=\"{2}\" AgentPT=\"{3}\" SuperMinPT=\"{4}\"/>";

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
            if (S4 > SettingType1Constants.MaxPositionTakingValue
                || M4 > SettingType1Constants.MaxPositionTakingValue
                || A4 > SettingType1Constants.MaxPositionTakingValue
                || N4 > SettingType1Constants.MaxPositionTakingValue)
            {
                return new Result(SettingType1Constants.PositionTakingExceedMaxValueErrorCode);
            }

            if (M4 > S4)
            {
                return new Result(SettingType1Constants.MasterMaxPTIsGreaterThanSuperMaxPTErrorCode);
            }

            if (A4 > M4)
            {
                return new Result(SettingType1Constants.AgentMaxPTIsGreaterThanMasterMaxPTErrorCode);
            }

            if (N4 > S4)
            {
                return new Result(SettingType1Constants.SuperMinPTIsGreaterThanSuperMaxPTErrorCode);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                PositionTakingXmlFormat,
                base.ToXml(),
                S4,
                M4,
                A4,
                N4);
        }
    }
}