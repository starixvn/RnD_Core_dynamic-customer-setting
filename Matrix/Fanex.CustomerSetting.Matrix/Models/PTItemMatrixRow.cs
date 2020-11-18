namespace Fanex.CustomerSetting.Matrix.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using CustomerSetting.Models.Enumerations;
    using Dtos;

    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules",
        "SA1305:FieldNamesMustNotUseHungarianNotation",
        Justification = "PT is abbriviation for Position Taking.")]
    public class PTItemMatrixRow
    {
        public PTItemMatrixRow()
        {
        }

        internal PTItemMatrixRow(PTItemMatrixDto pTMatrixDto)
        {
            if (pTMatrixDto == null)
            {
                throw new ArgumentNullException(nameof(pTMatrixDto), "PT Item Matrix Dto cannot be null");
            }

            CustomerLevel = Enumeration.FromValue<Level>(pTMatrixDto.LevelValue);

            SetSuperPT(pTMatrixDto);

            SetMasterPT(pTMatrixDto);

            AgentPT = CustomerLevel.IsMasterLevel() ? null : pTMatrixDto.AgentPT;

            MinPT = pTMatrixDto.MinPT;
        }

        public Level CustomerLevel { get; set; }

        public decimal? SuperPT { get; set; }

        public decimal? MasterPT { get; set; }

        public decimal? AgentPT { get; set; }

        public decimal? AutoSuperPT { get; set; }

        public bool? IsAutoSuperPT { get; set; }

        public decimal? AutoMasterPT { get; set; }

        public bool? IsAutoMasterPT { get; set; }

        public decimal? MinPT { get; set; }

        private void SetMasterPT(PTItemMatrixDto pTMatrixDto)
        {
            MasterPT = pTMatrixDto.MasterPT;

            if (CustomerLevel.IsAgentLevel() || CustomerLevel.IsMemberLevel())
            {
                IsAutoMasterPT = pTMatrixDto.IsAutoMasterPT;
                AutoMasterPT = IsAutoMasterPT.Value ? pTMatrixDto.AutoMasterPT : 0;

                if (CustomerLevel.IsAgentLevel())
                {
                    MasterPT = IsAutoMasterPT.Value ? 0 : pTMatrixDto.MasterPT;
                }
            }
        }

        private void SetSuperPT(PTItemMatrixDto pTMatrixDto)
        {
            if (CustomerLevel.IsMasterLevel() || CustomerLevel.IsMemberLevel())
            {
                IsAutoSuperPT = pTMatrixDto.IsAutoSuperPT;
                AutoSuperPT = IsAutoSuperPT.Value ? pTMatrixDto.AutoSuperPT : 0;

                if (CustomerLevel.IsMasterLevel())
                {
                    SuperPT = IsAutoSuperPT.Value ? 0 : pTMatrixDto.SuperPT;
                }
                else
                {
                    SuperPT = pTMatrixDto.SuperPT;
                }
            }
            else if (CustomerLevel.IsSuperLevel())
            {
                SuperPT = pTMatrixDto.SuperPT;
            }
        }
    }
}