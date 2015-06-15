namespace Fanex.CustomerSetting.Core.Results.Impl
{
    public class Result : IResult
    {
        public const int SuccessValue = 0;
        public const int ErrorValue = -1;
        public const int NoChangeValue = -2;
        public const int AccessDeniedValue = -3;

        public Result(Result result)
            : this(result.ErrCode, result.ErrMsg)
        {
        }

        public Result(int errCode, string errMsg)
        {
            ErrCode = errCode;
            ErrMsg = errMsg;
        }

        public static Result Success
        {
            get
            {
                return new Result(SuccessValue, string.Empty);
            }
        }

        public static Result AccessDenied
        {
            get
            {
                return new Result(AccessDeniedValue, "Lang.AccessDeniedMsg");
            }
        }

        public static Result NoChange
        {
            get
            {
                return new Result(NoChangeValue, "Lang.NoChangeToUpdate");
            }
        }

        public static Result Error
        {
            get
            {
                return new Result(ErrorValue, "Oops... you are here because of a system error, please notify your administrator.");
            }
        }

        public int ErrCode { get; private set; }

        public string ErrMsg { get; private set; }

        public bool IsSuccessful
        {
            get
            {
                return ErrCode == SuccessValue;
            }
        }
    }
}