namespace Fanex.CustomerSetting.Matrix.Criteria
{
    using Data.Repository;

    internal class CustomerLineQueryCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_CustomerLineInfoSelect";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName);
        }
    }
}