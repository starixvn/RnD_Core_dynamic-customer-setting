namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public class MasterOnMemberPositionTakingItem : BasePositionTakingItem
    {
        private const string PositionTakingXmlFormat = "<PT {0} MasterPT=\"{1}\" AutoMasterPT=\"{2}\" IsAutoMasterPT=\"{3}\"/>";

        public byte A1 { get; set; }

        public byte M1 { get; set; }

        public bool IM1 { get; set; }

        public byte UM1 { get; set; }

        public bool IM2 { get; set; }

        public byte M3 { get; set; }

        public byte N3 { get; set; }

        public override IResult Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.IsSuccessful)
            {
                return baseResult;
            }

            //// Will code more business here
            if ((IM1 && UM1 < 0)
                || (!IM1 && M1 < 0))
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
                M1,
                UM1,
                IM1 ? 1 : 0);
        }
    }
}