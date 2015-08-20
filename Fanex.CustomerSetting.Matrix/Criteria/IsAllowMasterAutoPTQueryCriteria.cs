namespace Fanex.CustomerSetting.Matrix.Criteria
{
    using Fanex.NAP.Framework.Data;

    internal class IsAllowMasterAutoPTQueryCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_AllowMasterPTSelect";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName);
        }
    }
}