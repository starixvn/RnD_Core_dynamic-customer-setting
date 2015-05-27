namespace Fanex.CustomerSetting.Matrix.Entities
{
    public class BaseMatrixSettingEntity
    {
        public byte RoleId { get; set; }

        public short SportType { get; set; }

        public string SportTypeName { get; set; }

        public short BetType { get; set; }

        public string BetTypeName { get; set; }
    }
}