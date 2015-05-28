namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    public class PermissionValidateMultipleCriteria : ResultCriteria
    {
        public int CustId { get; set; }

        [SpParam(SqlParamName = "TargetCustIDLst")]
        public string TargetCustIds { get; set; }

        public int TargetRoleId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_MultiCustPermissionValidate";
        }

        public override bool IsValid()
        {
            return CustId > 0 && !string.IsNullOrEmpty(TargetCustIds) && TargetRoleId > 0;
        }
    }
}