namespace Fanex.CustomerSetting.Criteria
{
    using Data.Repository;
    using Models.Enumerations;

    internal class CustomerInfoQueryCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        [SpParam(Ignored = true)]
        public byte System { get; set; }

        public override string GetSettingKey()
        {
            if (System == (byte)SystemType.ExternalNap || System == (byte)SystemType.ExternalNapBetSetting)
            {
                return "EAS_CustSetting_CustomerInfoSelect";
            }

            return "CustSetting_CustomerInfoSelect";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}