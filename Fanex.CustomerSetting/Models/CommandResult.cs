namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Enumerations;

    public class CommandResult : Result
    {
        public CommandResult(CommandType commandType, int errCode, string errMsg)
            : base(errCode, errMsg)
        {
            CommandType = commandType;
        }

        public CommandResult(CommandType commandType, Result result)
            : base(result)
        {
            CommandType = commandType;
        }

        public CommandResult(
            Result statusResult,
            Result discountResult,
            Result betLimitResult,
            Result positionTakingResult)
            : this(CommandType.All, Result.Success)
        {
            StatusResult = statusResult;
            DiscountResult = discountResult;
            BetLimitResult = betLimitResult;
            PositionTakingResult = positionTakingResult;
        }

        public CommandType CommandType { get; private set; }

        public Result StatusResult { get; private set; }

        public Result DiscountResult { get; private set; }

        public Result BetLimitResult { get; private set; }

        public Result PositionTakingResult { get; private set; }
    }
}