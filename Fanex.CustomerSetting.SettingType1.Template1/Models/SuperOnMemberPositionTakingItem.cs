namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public class SuperOnMemberPositionTakingItem : BasePositionTakingItem
    {
        private const string PositionTakingXmlFormat = "<PT {0} SuperPT=\"{1}\" AutoSuperPT=\"{2}\" IsAutoSuperPT=\"{3}\"/>";

        public byte A1 { get; set; }

        public byte M1 { get; set; }

        public byte S1 { get; set; }

        public bool IS1 { get; set; }

        public bool IS3 { get; set; }

        public byte US1 { get; set; }

        public byte S4 { get; set; }

        public byte N4 { get; set; }

        public byte M3 { get; set; }

        public override IResult Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.IsSuccessful)
            {
                return baseResult;
            }

            //// Will code more business here
            if ((IS1 && US1 < 0)
                || (!IS1 && S1 < 0))
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
                S1,
                US1,
                IS1 ? 1 : 0);
        }
    }
}