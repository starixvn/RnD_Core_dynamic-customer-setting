namespace Fanex.CustomerSetting.Core.SPObjects.Commands.Validation
{
    using Fanex.NAP.Framework.Data;
    public class PermissionValidationCommand : BaseCommand
    {
        [SpParam(SqlParamName = "CustId")]
        public int ActorId { get; set; }

        [SpParam(SqlParamName = "TargetCustId")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "TargetRoleId")]
        public int CustomerLevelId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_PermissionValidate";
        }

        public override bool IsValid()
        {
            return ActorId > 0 && CustomerId > 0 && CustomerLevelId > 0;
        }
    }
}