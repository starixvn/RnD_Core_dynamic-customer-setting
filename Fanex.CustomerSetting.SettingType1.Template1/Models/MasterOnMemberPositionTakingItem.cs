namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;

    public class MasterOnMemberPositionTakingItem : BasePositionTakingItem
    {
        private const string PositionTakingXmlFormat = "<PT {0} MasterPT=\"{1}\" AutoMasterPT=\"{2}\" IsAutoMasterPT=\"{3}\"/>";

        /// <summary>
        /// Agent PT on Member
        /// </summary>
        public byte A1 { get; set; }

        /// <summary>
        /// Master PT On member
        /// </summary>
        public byte M1 { get; set; }

        /// <summary>
        /// Is Master auto on Member
        /// </summary>
        public bool IM1 { get; set; }

        /// <summary>
        /// Super Auto PT on Member
        /// </summary>
        public byte UM1 { get; set; }

        /// <summary>
        /// Is master auto on Agent
        /// </summary>
        public bool IM2 { get; set; }

        /// <summary>
        /// Master Max PT
        /// </summary>
        public byte M3 { get; set; }

        /// <summary>
        /// Master Min PT
        /// </summary>
        public byte N3 { get; set; }

        public override IResult Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.IsSuccessful)
            {
                return baseResult;
            }

            //// Will code more business here
            if (!IM1 && UM1 > 0)
            {
                return new Result(SettingType1ErrorCodes.MasterAutoPTOnMemberIsGreaterThanZeroWhenIsAutoIsNotSet);
            }

            if (IM1 && M1 > 0)
            {
                return new Result(SettingType1ErrorCodes.MasterPTOnMemberIsGreaterThanZeroWhenIsAutoIsSet);
            }

            return Result.Success;
        }

        public override string ToXml()
        {
            return string.Format(
                PositionTakingXmlFormat,
                base.ToXml(),
                M1,
                UM1,
                IM1 ? 1 : 0);
        }
    }
}