namespace Fanex.CustomerSetting.Matrix.Dtos
{
    using Fanex.CustomerSetting.Enumerations;

    public class CommissionItemMatrixDto
    {
        public CustomerLevel CustomerLevel { get; set; }

        public short SportType { get; set; }

        public short BetType { get; set; }

        public decimal CommRate { get; set; }
    }
}