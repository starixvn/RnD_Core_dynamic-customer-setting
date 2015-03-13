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

        public UpdateResult(Result result)
            : base(result.ErrCode, result.ErrMsg)
        {
        }

        public UpdateResult(IList<Result> results)
            : this(GetResult(results))
        {
            Results = results;
        }

        public IList<Result> Results { get; set; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <param name="results">The list of results.</param>
        /// <returns>Returns status of results.</returns>
        private static Result GetResult(IList<Result> results)
        {
            if (results != null)
            {
                foreach (var result in results)
                {
                    if (!result.IsSuccess)
                    {
                        return result;
                    }
                }
            }

            return new Result(SuccessValue, string.Empty);
        }
    }
}