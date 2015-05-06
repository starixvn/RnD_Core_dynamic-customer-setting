namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// MultipleQueryResult class.
    /// </summary>
    public class MultipleCommandResult : Result
    {
        public IEnumerable<Result> Results { get; private set; }

        public MultipleCommandResult(Result result)
            : base(result)
        {
        }

        public MultipleCommandResult(IEnumerable<Result> results)
            : base(Result.Success)
        {
            Results = results;
        }
    }
}