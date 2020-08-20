namespace Fanex.CustomerSetting.Matrix.Models
{
    public class SettingMatrixQuery
    {
        public byte SettingTypeId { get; set; }

        // Cheat code to use settingTypeId replace for ViewMode as parameter
        // Due to cannot build Matrix Js file issue.
        public byte ViewMode => SettingTypeId;

        public byte ProductId { get; set; }

        public string CustomerName { get; set; }

        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public bool IsLive { get; set; }
    }
}