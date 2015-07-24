namespace Fanex.CustomerSetting.Results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Constants;
    using Resource = Fanex.CustomerSetting.Resource;

    public class Result : IResult
    {
        public static readonly Result Success = new Result(ResultValue.Success);
        public static readonly Result AccessDenied = new Result(ResultValue.AccessDenied, Resource.Message.AccessDeniedMsg);
        public static readonly Result NoChange = new Result(ResultValue.NoChange, Resource.Message.NoChangeToUpdate);
        public static readonly Result SystemError = new Result(ResultValue.Error, Resource.Message.SystemError);
        public static readonly Result DataError = new Result(ResultValue.DataIsNotValid, Resource.Message.DataIsNotValid);

        public Result(int code)
            : this(code, string.Empty)
        {
        }

        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public Result(int code, string message, int msgType)
            : this(code, message)
        {
            MsgType = msgType;
        }

        public int Code { get; private set; }

        public string Message { get; private set; }

        public int MsgType { get; private set; }

        public virtual bool IsSuccessful
        {
            get { return Code == ResultValue.Success; }
        }

        public virtual bool IsFailed
        {
            get { return !IsSuccessful;  }
        }

        public bool IsOk
        {
            get { return IsSuccessful;  }
        }

        public bool IsNotOk
        {
            get { return !IsOk; }
        }
    }
}