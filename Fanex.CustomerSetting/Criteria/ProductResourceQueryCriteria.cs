namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    public class ProductResourceQueryCriteria : CriteriaBase
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