namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public class AgentPositionTakingItem : SportTypeBetTypeSettingItem
    {
        private const string PositionTakingXmlFormat = "<PT {0} MasterPT=\"{1}\" IsAutoMasterPT=\"{2}\" AutoMasterPT=\"{3}\" AgentPT=\"{4}\" AgentMinPT=\"{5}\" UpdAllMems=\"{6}\" NewMasterPT=\"{7}\"/>";

        public byte M2 { get; set; }

        public byte UM2 { get; set; }

        public bool IM2 { get; set; }

        public byte A2 { get; set; }

        public byte N2 { get; set; }

        public byte M3 { get; set; }

        public byte N3 { get; set; }

        public byte A4 { get; set; }

        public bool IsUpdateMasterPTOnMembers { get; set; }

        public byte MasterPTOnMembers { get; set; }

        public override IResult Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.IsSuccessful)
            {
                return baseResult;
            }

            //// Will code more business here
            if (M2 < 0
                || UM2 < 0
                || A2 < 0
                || N2 < 0
                || MasterPTOnMembers < 0)
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
                M2,
                IM2,
                UM2,
                A2,
                N2,
                IsUpdateMasterPTOnMembers ? 1 : 0,
                MasterPTOnMembers);
        }
    }
}