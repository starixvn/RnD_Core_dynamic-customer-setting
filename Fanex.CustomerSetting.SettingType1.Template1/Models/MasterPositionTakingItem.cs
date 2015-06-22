namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public class MasterPositionTakingItem : BasePositionTakingItem
    {
        private const string PositionTakingXmlFormat = "<PT {0} SuperPT=\"{1}\" IsAutoSuperPT=\"{2}\" AutoSuperPT =\"{3}\" MasterPT=\"{4}\" MasterMinPT=\"{5}\" UpdAllMems=\"{6}\" NewSuperPT=\"{7}\"/>";

        public byte S3 { get; set; }

        public byte US3 { get; set; }

        public bool IS3 { get; set; }

        public byte M3 { get; set; }

        public byte N3 { get; set; }

        public byte S4 { get; set; }

        public byte M4 { get; set; }

        public byte N4 { get; set; }

        public bool IsUpdateSuperPTOnMembers { get; set; }

        public byte SuperPTOnMembers { get; set; }

        public override IResult Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.IsSuccessful)
            {
                return baseResult;
            }

            //// Will code more business here
            if (S3 < 0 || US3 < 0 || M3 < 0 || N3 < 0 || SuperPTOnMembers < 0)
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
                S3,
                IS3,
                US3,
                M3,
                N3,
                IsUpdateSuperPTOnMembers ? 1 : 0,
                SuperPTOnMembers);
        }
    }
}