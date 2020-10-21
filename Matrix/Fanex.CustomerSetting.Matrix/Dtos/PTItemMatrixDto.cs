namespace Fanex.CustomerSetting.Matrix.Dtos
{
    internal class PTItemMatrixDto
    {
        public short SportType { get; set; }

        public short BetType { get; set; }

        public bool IsLive { get; set; }

        public byte LevelValue { get; set; }

        public decimal? SuperPT { get; set; }

        public decimal? MasterPT { get; set; }

        public decimal? AgentPT { get; set; }

        public decimal? AutoSuperPT { get; set; }

        public bool? IsAutoSuperPT { get; set; }

        public decimal? AutoMasterPT { get; set; }

        public bool? IsAutoMasterPT { get; set; }

        public decimal? MinPT { get; set; }
    }
}