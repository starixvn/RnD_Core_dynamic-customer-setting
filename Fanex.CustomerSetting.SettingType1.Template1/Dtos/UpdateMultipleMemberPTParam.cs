namespace Fanex.CustomerSetting.SettingType1.Template1.Dtos
{
    using Fanex.CustomerSetting.Enumerations;

    public class UpdateMultipleMemberPTParam
    {
        public string CustIds { get; set; }

        public int SubAccId { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public string PTXml { get; set; }

        public CustomerLevel UplineLevel { get; set; }
    }
}