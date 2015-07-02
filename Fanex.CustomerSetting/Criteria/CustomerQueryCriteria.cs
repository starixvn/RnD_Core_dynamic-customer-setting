namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class CustomerQueryCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_UserInfoSelect";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}