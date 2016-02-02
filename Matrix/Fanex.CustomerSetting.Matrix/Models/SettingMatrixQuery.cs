namespace Fanex.CustomerSetting.Matrix.Models
{
    public class SettingMatrixQuery
    {
        public byte SettingTypeId { get; set; }

        public byte ProductId { get; set; }

        public string CustomerName { get; set; }

        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public bool IsLive { get; set; }
    }
}