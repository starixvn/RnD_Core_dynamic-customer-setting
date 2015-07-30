namespace Fanex.CustomerSetting.Matrix.Models
{
    using System;
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Models.Enumerations;

    public class CommissionItemMatrixRow
    {
        public CommissionItemMatrixRow()
        {
        }

        public CommissionItemMatrixRow(CommissionItemMatrixDto commissionItemMatrixDto)
        {
            if (commissionItemMatrixDto == null)
            {
                throw new ArgumentNullException("commissionItemMatrixDto", "Commission Item Matrix Dto cannot be null");
            }

            CustomerLevel = Enumeration.FromValue<Level>(commissionItemMatrixDto.LevelValue);
            CommRate = commissionItemMatrixDto.CommRate;
        }

        public Level CustomerLevel { get; set; }

        public decimal CommRate { get; set; }
    }
}