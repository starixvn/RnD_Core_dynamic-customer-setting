namespace Fanex.CustomerSetting.Criteria
{
    /// <summary>
    /// MasterSettingSelectCriteria class.
    /// </summary>
    public class SuperSelectMasterCriteria : SelectCriteria
    {
        public SuperSelectMasterCriteria(int custId, short productId)
            : base(custId, productId)
        {
        }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_MasterSettingSelect";
        }
    }
}