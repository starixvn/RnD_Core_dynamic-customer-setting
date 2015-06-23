namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

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
            var baseResult = base.Validate();
            if (!baseResult.IsSuccessful)
            {
                return baseResult;
            }

            return Result.Success;
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