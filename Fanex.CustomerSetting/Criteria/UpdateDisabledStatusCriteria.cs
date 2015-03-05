namespace Fanex.CustomerSetting.Criteria
{
    /// <summary>
    /// UpdateStatusCriteria class.
    /// </summary>
    public class UpdateDisabledStatusCriteria : UpdateCriteria
    {
        public string DisabledStatus { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && !string.IsNullOrEmpty(DisabledStatus);
        }
    }
}