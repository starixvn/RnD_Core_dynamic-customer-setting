namespace Fanex.CustomerSetting.Criteria
{
    /// <summary>
    /// MasterSettingSelectCriteria class.
    /// </summary>
    public class SuperSelectMasterCriteria : SelectCriteria
    {
        public SuperSelectMasterCriteria(int custId, short productId, string settingKey)
            : base(custId, productId, settingKey)
        {
        }
    }
}