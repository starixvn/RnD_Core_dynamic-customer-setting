namespace Fanex.CustomerSetting.Matrix.Criteria
{
    using Data.Repository;

    internal class PTMatrixQueryCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        public string UserName { get; set; }

        public short SportType { get; set; }

        public short BetType { get; set; }

        public bool IsLive { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_PTMatrix";
        }

        public override bool IsValid()
        {
            return SportType > 0
                && BetType > 0
                && !string.IsNullOrEmpty(UserName);
        }
    }
}