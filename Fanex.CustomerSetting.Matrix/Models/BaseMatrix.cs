namespace Fanex.CustomerSetting.Matrix.Models
{
    public class BaseMatrix
    {
        public short BetTypeId { get; set; }

        public string BetTypeName { get; set; }

        public short SportTypeId { get; set; }

        public string SportTypeName { get; set; }

        public byte SettingLevel { get; set; }
    }
}