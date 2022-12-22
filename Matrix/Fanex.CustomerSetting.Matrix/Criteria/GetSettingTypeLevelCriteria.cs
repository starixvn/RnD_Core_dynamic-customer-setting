namespace Fanex.CustomerSetting.Matrix.Criteria
{
    using Data.Repository;
    using Fanex.CustomerSetting.Configurations;

    internal class GetSettingTypeLevelCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        [SpParam(SqlParamName = "SportType")]
        public short SportTypeId { get; set; }

        [SpParam(SqlParamName = "SettingType")]
        public short SettingTypeId { get; set; }

        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey()
        {
            return "CustSetting_SettingLevel";
        }

        public override bool IsValid()
        {
            return SettingTypeId > 0 && (ProductId >= 0 || SportTypeId >= 0);
        }
    }
}