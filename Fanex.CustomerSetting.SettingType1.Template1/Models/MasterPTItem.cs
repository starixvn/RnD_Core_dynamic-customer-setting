namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;
    using Fanex.CustomerSetting.SettingType1.Template1.Resources;

    public class MasterPTItem : PTItem
    {
        private const string PTXmlFormat = "<PT {0} SuperPT=\"{1}\" IsAutoSuperPT=\"{2}\" AutoSuperPT=\"{3}\" MasterPT=\"{4}\" MasterMinPT=\"{5}\" UpdAllMems=\"{6}\" NewSuperPT=\"{7}\"/>";

        /// <summary>
        /// Super PT
        /// </summary>
        public byte S3 { get; set; }

        /// <summary>
        /// Super Auto PT
        /// </summary>
        public byte US3 { get; set; }

        /// <summary>
        /// Is Auto
        /// </summary>
        public bool IS3 { get; set; }

        /// <summary>
        /// Master Max PT
        /// </summary>
        public byte M3 { get; set; }

        /// <summary>
        /// Master Min PT
        /// </summary>
        public byte N3 { get; set; }

        /// <summary>
        /// Super Max PT
        /// </summary>
        public byte S4 { get; set; }

        /// <summary>
        /// Master Max PT
        /// </summary>
        public byte M4 { get; set; }

        /// <summary>
        /// Super Min PT
        /// </summary>
        public byte N4 { get; set; }

        public bool IsUpdateSuperPTOnMembers { get; set; }

        public byte SuperPTOnMembers { get; set; }

        public override IResult Validate()
        {
            if (S3 > Constants.MaxPTValue
                || US3 > Constants.MaxPTValue
                || N3 > Constants.MaxPTValue
                || M3 > Constants.MaxPTValue)
            {
                return new Result(
                    ErrorCode.PTExceedMaxValue,
                    PTMessage.PositionTakingCannotBeExceedMaxValue);
            }

            if (IS3 && S3 > 0)
            {
                return new Result(
                    ErrorCode.SuperPTIsGreaterThanZeroWhenIsAutoIsSet,
                    PTMessage.SuperPTCannotBeGreaterThanZeroWhenIsAutoIsSet);
            }

            if (!IS3 && US3 > 0)
            {
                return new Result(
                    ErrorCode.SuperAutoPTIsGreaterThanZeroWhenIsAutoIsNotSet,
                    PTMessage.SuperAutoPTCannotBeGreaterThanZeroWhenIsAutoIsNotSet);
            }

            if (N3 > M3)
            {
                return new Result(
                    ErrorCode.MasterMinPTIsGreaterThanMasterMaxPT,
                    PTMessage.MasterMinPTCannotBeGreaterThanMasterMaxPT);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                PTXmlFormat,
                base.ToXml(),
                S3,
                IS3 ? 1 : 0,
                US3,
                M3,
                N3,
                IsUpdateSuperPTOnMembers ? 1 : 0,
                SuperPTOnMembers);
        }
    }
}