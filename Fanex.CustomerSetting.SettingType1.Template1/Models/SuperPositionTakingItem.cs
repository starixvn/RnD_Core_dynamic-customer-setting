namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public class SuperPositionTakingItem : BasePositionTakingItem
    {
        private const string PositionTakingXmlFormat = "<PT {0} SuperPT=\"{1}\" MasterPT =\"{2}\" AgentPT = \"{3}\" SuperMinPT=\"{4}\"/>";

        public byte S4 { get; set; }

        public byte M4 { get; set; }

        public byte A4 { get; set; }

        public byte N4 { get; set; }

        public byte DefaultMaxPT { get; set; }

        public override IResult Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.IsSuccessful)
            {
                return baseResult;
            }

            //// Will code more business here
            if (S4 < 0
                || M4 < 0
                || A4 < 0
                || N4 < 0
                || M4 > S4
                || A4 > M4
                || N4 > S4)
            {
                return Result.DataError;
            }

            return Result.Success;
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