namespace Fanex.CustomerSetting.NumberGames2.Criteria
{
    using Data.Repository;

    internal class DirectUplineCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerDirectUplineSel";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}