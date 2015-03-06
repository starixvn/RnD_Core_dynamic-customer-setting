namespace Fanex.CustomerSetting.Criteria
{
    /// <summary>
    /// UpdateStatusCriteria class.
    /// </summary>
    public class UpdateDisabledStatusCriteria : UpdateCriteria
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