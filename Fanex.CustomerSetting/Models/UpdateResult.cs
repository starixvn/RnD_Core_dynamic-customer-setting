namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// UpdateResult class.
    /// </summary>
    public class UpdateResult : Result
    {
        public UpdateResult(int errCode, string errMsg)
            : base(errCode, errMsg)
        {
        }

        public UpdateResult(IEnumerable<Result> results)
            : this(Result.SuccessValue, string.Empty)
        {
            Results = results;
        }

        public IEnumerable<Result> Results { get; set; }
    }
}