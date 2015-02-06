namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// SettingSelectCriteria class.
    /// </summary>
    public abstract class SelectCriteria : CriteriaBase
    {
        private string _settingKey;

        protected SelectCriteria(int custId, short productId, string settingKey)
        {
            CustId = custId;
            ProductId = productId;
            _settingKey = settingKey;
        }

        public int CustId { get; private set; }

        public short ProductId { get; private set; }

        public override string GetSettingKey()
        {
            return _settingKey;
        }

        public override bool IsValid()
        {
            return CustId > 0 && ProductId > 0;
        }
    }
}