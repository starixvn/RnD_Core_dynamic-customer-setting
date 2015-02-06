namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// ProductResourceSelectCriteria class.
    /// </summary>
    public class ProductResourceSelectCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_CustSetting_ProductNamesSelect";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}