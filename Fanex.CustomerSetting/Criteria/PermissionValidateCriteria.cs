namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// PermissionValidateCriteria class.
    /// </summary>
    public class PermissionValidateCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int TargetCustId { get; set; }

        public int TargetRoleId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

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