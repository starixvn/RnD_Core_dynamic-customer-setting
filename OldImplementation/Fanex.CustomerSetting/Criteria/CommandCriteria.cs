namespace Fanex.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    public class CommandCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int SubAccId { get; set; }

        public int CustId { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public int MsgType { get; set; }

        public override bool IsValid()
        {
            return UserId > 0 && !(UserId == 0 && SubAccId > 0) && CustId > 0;
        }
    }
}