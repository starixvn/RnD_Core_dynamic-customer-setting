namespace Fanex.CustomerSetting.Dtos.Results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Constants;
    using ResourceMessage = Fanex.CustomerSetting.Resource.Resources;

    public class Result : IResult
    {
        public static readonly Result Success = new Result(ResultValue.Success);
        public static readonly Result AccessDenied = new Result(ResultValue.AccessDenied, ResourceMessage.Message.AccessDeniedMsg);
        public static readonly Result NoChange = new Result(ResultValue.NoChange, ResourceMessage.Message.NoChangeToUpdate);
        public static readonly Result SystemError = new Result(ResultValue.Error, ResourceMessage.Message.SystemError);
        public static readonly Result DataError = new Result(ResultValue.DataIsNotValid, ResourceMessage.Message.DataIsNotValid);

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

            Id = id;
            Name = name;
        }

        public Result(IEnumerable<IResult> subResults)
        {
            SubResults = subResults;
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

                return Code == ResultValue.Success;
            }
        }
    }
}