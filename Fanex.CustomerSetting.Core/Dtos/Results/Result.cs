namespace Fanex.CustomerSetting.Core.Dtos.Results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Core.Resources;

    public class Result : IResult
    {
        public const int SuccessValue = 0;
        public const int ErrorValue = -1;
        public const int NoChangeValue = -2;
        public const int AccessDeniedValue = -3;
        public const int DataIsNotValid = -4;

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

        public Result(int code, string message, IEnumerable<string> subMessages)
            : this(code, message)
        {
            SubMessages = subMessages;
        }

        public Result(int code, string message, IEnumerable<IResult> subResults)
            : this(code, message)
        {
            SubResults = subResults;
        }

        public Result(
            int code,
            string message,
            int msgType,
            IEnumerable<string> subMessages,
            IEnumerable<IResult> subResults)
            : this(code, message, msgType)
        {
            SubMessages = subMessages;
            SubResults = subResults;
        }

        public Result(int id, string name, IResult result)
        {
            Id = id;
            Name = name;

            if (result != null)
            {
                Code = result.Code;
                Message = result.Message;
                SubMessages = result.SubMessages;
                SubResults = result.SubResults;
            }
            else
            {
                throw new ArgumentNullException("result");
            }
        }

        public Result(IEnumerable<IResult> subResults)
        {
            SubResults = subResults;
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
                return new Result(AccessDeniedValue, Lang.AccessDeniedMsg);
            }
        }

        public static Result NoChange
        {
            get
            {
                return new Result(NoChangeValue, Lang.NoChangeToUpdate);
            }
        }

        public static Result SystemError
        {
            get
            {
                return new Result(ErrorValue, Lang.SystemError);
            }
        }

        public static Result DataError
        {
            get
            {
                return new Result(DataIsNotValid, Lang.DataIsNotValid);
            }
        }

        public int Code { get; private set; }

        public string Message { get; private set; }

        public int MsgType { get; private set; }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<string> SubMessages { get; private set; }

        public IEnumerable<IResult> SubResults { get; private set; }

        public bool IsSuccessful
        {
            get
            {
                if (SubResults != null && SubResults.Any())
                {
                    return SubResults.Where(x => !x.IsSuccessful).FirstOrDefault() == null;
                }

                return Code == SuccessValue;
            }
        }
    }
}