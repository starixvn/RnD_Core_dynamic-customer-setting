namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// SettingSelectCriteria class.
    /// </summary>
    public abstract class SelectCriteria : CriteriaBase
    {
        protected SelectCriteria(int custId, short productId)
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