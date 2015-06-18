namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    public abstract class QueryCriteria : CriteriaBase
    {
        protected QueryCriteria(int custId, short productId)
        {
            CustId = custId;
            ProductId = productId;
        }

        public int CustId { get; private set; }

        public short ProductId { get; private set; }

        public override bool IsValid()
        {
            return CustId > 0 && ProductId > 0;
        }
    }
}