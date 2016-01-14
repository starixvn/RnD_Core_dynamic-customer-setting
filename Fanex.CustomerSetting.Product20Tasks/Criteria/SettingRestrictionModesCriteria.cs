namespace Fanex.CustomerSetting.Product20Tasks.Criteria
{
    using Fanex.Data.Repository;

    public class SettingRestrictionModesCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        public byte SystemId { get; set; }

        public int TargetId { get; set; }

        public byte TargetRoleId { get; set; }

        public byte ActorRoleId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_UpdateRestrictionSelect";
        }

        public override bool IsValid()
        {
            return SystemId > 0
                && TargetId > 0
                && TargetRoleId > 0
                && ActorRoleId > 0;
        }
    }
}