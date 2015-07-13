namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class CustomerValidationInfoQueryCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_UserValidationInfoSelect";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}