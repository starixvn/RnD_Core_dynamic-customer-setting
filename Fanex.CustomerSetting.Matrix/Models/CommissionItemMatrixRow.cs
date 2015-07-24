namespace Fanex.CustomerSetting.Matrix.Models
{
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Models.Enumerations;

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

        public Level CustomerLevel { get; set; }

        public decimal CommRate { get; set; }
    }
}