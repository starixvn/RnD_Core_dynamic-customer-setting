namespace Fanex.CustomerSetting.Matrix.Models
{
    using System;
    using CustomerSetting.Models.Enumerations;
    using Dtos;

    public class CommissionItemMatrixRow
    {
        public CommissionItemMatrixRow()
        {
        }

        internal CommissionItemMatrixRow(CommissionItemMatrixDto commissionItemMatrixDto)
        {
            if (commissionItemMatrixDto == null)
            {
                throw new ArgumentNullException(nameof(commissionItemMatrixDto), "Commission Item Matrix Dto cannot be null");
            }

            CustomerLevel = Enumeration.FromValue<Level>(commissionItemMatrixDto.LevelValue);
            CommRate = commissionItemMatrixDto.CommRate;
        }

        public Level CustomerLevel { get; set; }

        public decimal? CommRate { get; set; }
    }
}