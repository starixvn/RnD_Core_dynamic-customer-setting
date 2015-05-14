namespace Fanex.CustomerSetting.Criteria
{
    public class DisabledStatusCommandCriteria : CommandCriteria
    {
        public string IsDisabled { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_DisableStatUpd";
        }

        public override bool IsValid()
        {
            return base.IsValid() && !string.IsNullOrEmpty(IsDisabled);
        }
    }
}