namespace Fanex.CustomerSetting.Matrix.Entities
{
    public class PositionTakingMatrixSettingEntity
    {
        public short SportType { get; set; }

        public short BetType { get; set; }

        public bool IsLive { get; set; }

        public byte RoleId { get; set; }

        public byte SuperPT { get; set; }

        public byte AutoSuperPT { get; set; }

        public bool IsAutoSuperPT { get; set; }

        public byte MasterPT { get; set; }

        public byte AutoMasterPT { get; set; }

        public bool IsAutoMasterPT { get; set; }

        public byte MinPT { get; set; }

        public byte MaxPT { get; set; }
    }
}