namespace Fanex.CustomerSetting.Matrix.Dtos
{
    using Fanex.CustomerSetting.Enumerations;

    public class PTMatrixDto
    {
        public short SportType { get; set; }

        public short BetType { get; set; }

        public bool IsLive { get; set; }

        public CustomerLevel CustomerLevel { get; set; }

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