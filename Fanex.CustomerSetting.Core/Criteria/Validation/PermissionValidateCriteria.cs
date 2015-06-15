namespace Fanex.CustomerSetting.Core.Criteria.Validation
{
    public class PermissionValidateCriteria : ResultCriteria
    {
        public int CustId { get; set; }

        public int TargetCustId { get; set; }

        public int TargetRoleId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_PermissionValidate";
        }

        public override bool IsValid()
        {
            return CustId > 0 && TargetCustId > 0 && TargetRoleId > 0;
        }
    }
}