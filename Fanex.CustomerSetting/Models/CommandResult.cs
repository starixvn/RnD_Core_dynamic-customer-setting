namespace Fanex.CustomerSetting.Models
{
    /// <summary>
    /// UpdateResult class.
    /// </summary>
    public class CommandResult : Result
    {
        public CommandResult(int errCode, string errMsg)
            : base(errCode, errMsg)
        {
        }

        public CommandResult(Result result)
            : base(result)
        {
        }

        public CommandResult(
            Result statusResult,
            Result discountResult,
            Result betLimitResult,
            Result positionTakingResult)
            : this(Result.Success)
        {
            StatusResult = statusResult;
            DiscountResult = discountResult;
            BetLimitResult = betLimitResult;
            PositionTakingResult = positionTakingResult;
        }

        public Result StatusResult { get; private set; }

        public Result DiscountResult { get; private set; }

        public Result BetLimitResult { get; private set; }

        public Result PositionTakingResult { get; private set; }
    }
}