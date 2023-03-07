namespace Fanex.CustomerSetting.Matrix.Criteria
{
    using Data.Repository;
    using Fanex.CustomerSetting.Configurations;

    internal class GetSettingTypeIdCriteria : CriteriaBase
    {
        public byte? ProductId { get; set; }

        [SpParam(SqlParamName = "SportType")]
        public short? SportTypeId { get; set; }

        public byte Mode { get; set; }

        public byte SettingType { get; set; }

        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey()
        {
            return "CustSetting_ViewMatrixInfo";
        }

        public override bool IsValid()
        {
            return ProductId >= 0 || SportTypeId >= 0;
        }
    }
}