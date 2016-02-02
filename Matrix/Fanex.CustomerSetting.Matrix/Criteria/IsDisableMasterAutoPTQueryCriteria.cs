namespace Fanex.CustomerSetting.Matrix.Criteria
{
    using Fanex.Data.Repository;

    internal class IsDisableMasterAutoPTQueryCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_DisableMasterAutoPTSelect";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName);
        }
    }
}