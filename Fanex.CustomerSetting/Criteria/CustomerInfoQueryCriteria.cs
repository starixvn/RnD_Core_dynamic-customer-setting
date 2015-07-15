namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class CustomerInfoQueryCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_UserInfoSelect";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}