namespace Fanex.CustomerSetting.Core.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class ResultCommand : CriteriaBase
    {
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}