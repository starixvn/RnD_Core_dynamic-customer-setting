namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;

    public class AgentOnMemberPositionTakingItem : BasePositionTakingItem
    {
        private const string PositionTakingXmlFormat = "<PT {0} AgentPT=\"{1}\"/>";

        /// <summary>
        /// Agent PT
        /// </summary>
        public byte A1 { get; set; }

        /// <summary>
        /// Agent Max PT
        /// </summary>
        public byte A2 { get; set; }

        /// <summary>
        /// Agent Min PT
        /// </summary>
        public byte N2 { get; set; }

        public override IResult Validate()
        {
            if (A1 > SettingType1Constants.MaxPositionTakingValue)
            {
                return new Result(SettingType1Constants.PositionTakingExceedMaxValueErrorCode);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                PositionTakingXmlFormat,
                base.ToXml(),
                A1);
        }
    }
}