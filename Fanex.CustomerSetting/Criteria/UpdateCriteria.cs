namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// UpdateCriteria class.
    /// </summary>
    public class UpdateCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int SubAccId { get; set; }

        public int CustId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public int ErrMsg { get; set; }

        public override bool IsValid()
        {
            return UserId > 0 && !(UserId == 0 && SubAccId > 0) && CustId > 0 && !string.IsNullOrEmpty(Language);
        }
    }
}