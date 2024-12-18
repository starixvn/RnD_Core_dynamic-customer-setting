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

        internal PTItemMatrixRow(PTItemMatrixDto pTMatrixDto, DirectMember directMemberEnum)
        {
            if (pTMatrixDto == null)
            {
                throw new ArgumentNullException(nameof(pTMatrixDto), "PT Item Matrix Dto cannot be null");
            }

            DirectMember = directMemberEnum;

            CustomerLevel = Enumeration.FromValue<Level>(pTMatrixDto.LevelValue);

            SetSuperPT(pTMatrixDto);

            SetMasterPT(pTMatrixDto);

            AgentPT = CustomerLevel.IsMasterLevel() || (DirectMember != DirectMember.None && CustomerLevel.IsNotSuperLevel()) ? null : pTMatrixDto.AgentPT;

            MinPT = DirectMember == DirectMember.SuperDirectMember && CustomerLevel.IsMemberLevel() ? null : pTMatrixDto.MinPT;
        }

        public Level CustomerLevel { get; set; }

        public DirectMember DirectMember { get; set; }

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
            if (DirectMember != DirectMember.SuperDirectMember || CustomerLevel.IsSuperLevel())
            {
                MasterPT = pTMatrixDto.MasterPT;
            }

            if (DirectMember == DirectMember.None && (CustomerLevel.IsAgentLevel() || CustomerLevel.IsMemberLevel()))
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
            if (DirectMember == DirectMember.SuperDirectMember && CustomerLevel.IsMemberLevel())
            {
                SuperPT = pTMatrixDto.SuperPT;
            }
            else if (CustomerLevel.IsMasterLevel() || CustomerLevel.IsMemberLevel())
            {
                IsAutoSuperPT = pTMatrixDto.IsAutoSuperPT;
                AutoSuperPT = IsAutoSuperPT.Value ? pTMatrixDto.AutoSuperPT : 0;

#pragma warning disable S3358 // Ternary operators should not be nested
                SuperPT = CustomerLevel.IsMasterLevel() ? IsAutoSuperPT.Value ? 0 : pTMatrixDto.SuperPT : pTMatrixDto.SuperPT;
#pragma warning restore S3358 // Ternary operators should not be nested
            }
#pragma warning disable S126 // "if ... else if" constructs should end with "else" clauses
            else if (CustomerLevel.IsSuperLevel())
#pragma warning restore S126 // "if ... else if" constructs should end with "else" clauses
            {
                SuperPT = pTMatrixDto.SuperPT;
            }
        }
    }
}