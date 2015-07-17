namespace Fanex.CustomerSetting.Matrix.Models
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Matrix.Dtos;

    public class PTItemMatrixRow
    {
        public PTItemMatrixRow()
        {
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "PT is abbriviation for Position Taking.")]
        public PTItemMatrixRow(PTItemMatrixDto pTMatrixDto)
        {
            CustomerLevel = pTMatrixDto.CustomerLevel;
            SuperPT = pTMatrixDto.SuperPT;
            MasterPT = pTMatrixDto.MasterPT;
            AgentPT = pTMatrixDto.AgentPT;
            AutoSuperPT = pTMatrixDto.AutoSuperPT;
            IsAutoSuperPT = pTMatrixDto.IsAutoSuperPT;
            AutoMasterPT = pTMatrixDto.AutoMasterPT;
            IsAutoMasterPT = pTMatrixDto.IsAutoMasterPT;
            MinPT = pTMatrixDto.MinPT;
        }

        public UserLevel CustomerLevel { get; set; }

        public byte? SuperPT { get; set; }

        public byte? MasterPT { get; set; }

        public byte? AgentPT { get; set; }

        public byte? AutoSuperPT { get; set; }

        public bool? IsAutoSuperPT { get; set; }

        public byte? AutoMasterPT { get; set; }

        public bool? IsAutoMasterPT { get; set; }

        public byte? MinPT { get; set; }
    }
}