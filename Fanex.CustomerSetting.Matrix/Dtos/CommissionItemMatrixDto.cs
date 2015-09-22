namespace Fanex.CustomerSetting.Matrix.Dtos
{
    internal class CommissionItemMatrixDto
    {
        public byte LevelValue { get; set; }

        public short SportType { get; set; }

        public short BetType { get; set; }

        public decimal CommRate { get; set; }
    }
}