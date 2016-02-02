namespace Fanex.CustomerSetting.Matrix.Criteria
{
    using Fanex.Data.Repository;

    internal class GetSettingTypeLevelCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        [SpParam(SqlParamName = "SportType")]
        public short SportTypeId { get; set; }

        [SpParam(SqlParamName = "SettingType")]
        public short SettingTypeId { get; set; }

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