namespace Fanex.CustomerSetting.Core.SPObjects.Commands.Validation
{
    using Fanex.NAP.Framework.Data;

    public class PermissionValidationMultipleCommand : BaseCommand
    {
        [SpParam(SqlParamName = "CustId")]
        public int ActorId { get; set; }

        [SpParam(SqlParamName = "TargetCustIDLst")]
        public string MemberIds { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_MultiCustPermissionValidate";
        }

        public override bool IsValid()
        {
            return ActorId > 0 && !string.IsNullOrEmpty(MemberIds);
        }
    }
}