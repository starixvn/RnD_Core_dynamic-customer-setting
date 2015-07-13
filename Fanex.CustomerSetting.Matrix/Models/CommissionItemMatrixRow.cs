namespace Fanex.CustomerSetting.Matrix.Models
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Matrix.Dtos;

    public class CommissionItemMatrixRow
    {
        public CommissionItemMatrixRow()
        {
        }

        public CommissionItemMatrixRow(CommissionItemMatrixDto commissionItemMatrixDto)
        {
            CustomerLevel = commissionItemMatrixDto.CustomerLevel;
            CommRate = commissionItemMatrixDto.CommRate;
        }

        public CustomerLevel CustomerLevel { get; set; }

        public decimal CommRate { get; set; }
    }
}