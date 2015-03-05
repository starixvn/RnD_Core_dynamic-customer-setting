namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Resources;

    /// <summary>
    /// Result class.
    /// </summary>
    public class Result
    {
        public const int SuccessValue = 0;
        public const int FailureValue = -1;

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
                return new Result(FailureValue, Lang.AccessDeniedMsg);
            }
        }

        public static Result NoChange
        {
            get
            {
                return new Result(FailureValue, Lang.NoChangeToUpdate);
            }
        }

        public int ErrCode { get; private set; }

        public string ErrMsg { get; private set; }

        public bool IsSuccess
        {
            get
            {
                return ErrCode == SuccessValue;
            }
        }
    }
}