namespace Fanex.CustomerSetting.Core.Dtos.Results.Impl
{
    using System;
    using System.Collections.Generic;

    public class Result : IResult
    {
        public const int SuccessValue = 0;
        public const int ErrorValue = -1;
        public const int NoChangeValue = -2;
        public const int AccessDeniedValue = -3;

        public Result(Result result)
            : this(result.Code, result.Message, result.SubMessages)
        {
        }

        public Result(int errCode, string errMsg, IEnumerable<string> subMessages = null)
        {
            Code = errCode;
            Message = errMsg;
            SubMessages = subMessages;
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

        public int Code { get; private set; }

        public string Message { get; private set; }

        public int MsgType { get; private set; }

        public bool IsSuccessful
        {
            get
            {
                return Code == SuccessValue;
            }
        }

        public IEnumerable<string> SubMessages { get; private set; }
    }
}