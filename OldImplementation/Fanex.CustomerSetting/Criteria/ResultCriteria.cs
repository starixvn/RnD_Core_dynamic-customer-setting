namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    public abstract class ResultCriteria : CriteriaBase
    {
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}