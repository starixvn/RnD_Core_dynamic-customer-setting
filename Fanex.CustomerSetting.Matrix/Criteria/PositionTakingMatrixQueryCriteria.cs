namespace Fanex.CustomerSetting.Matrix.Criteria
{
    using Fanex.Data.Repository;

    public class PositionTakingMatrixQueryCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public int ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_PTMatrix";
        }

        public override bool IsValid()
        {
            return ProductId > 0;
        }
    }
}