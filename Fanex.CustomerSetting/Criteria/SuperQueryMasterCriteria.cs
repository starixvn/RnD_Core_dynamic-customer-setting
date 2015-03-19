namespace Fanex.CustomerSetting.Criteria
{
    /// <summary>
    /// SuperQueryMasterCriteria class.
    /// </summary>
    public class SuperQueryMasterCriteria : QueryCriteria
    {
        public SuperQueryMasterCriteria(int custId, short productId)
            : base(custId, productId)
        {
        }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_MasterSettingSelect";
        }
    }
}