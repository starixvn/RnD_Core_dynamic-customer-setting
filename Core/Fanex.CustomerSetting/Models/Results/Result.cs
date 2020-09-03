namespace Fanex.CustomerSetting.Models.Results
{
    using Constants;
    using Resource = Resource;

    public class Result : IResult
    {
        public static readonly Result Success = new Result(ResultValues.Success);
        public static readonly Result AccessDenied = new Result(ResultValues.AccessDenied, Resource.Message.AccessDeniedMsg, MessageTypes.Exception);
        public static readonly Result NoChange = new Result(ResultValues.NoChange, Resource.Message.NoChangeToUpdate, MessageTypes.Exception);
        public static readonly Result SystemError = new Result(ResultValues.Error, Resource.Message.SystemError, MessageTypes.Exception);
        public static readonly Result DataError = new Result(ResultValues.DataIsNotValid, Resource.Message.DataIsNotValid, MessageTypes.Exception);

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
            get { return Code == ResultValues.Success || Code == ResultValues.SuccessButDoNothingBecauseUM; }
        }

        public virtual bool IsFailed
        {
            get { return !IsSuccessful; }
        }

        public bool IsOk
        {
            get { return IsSuccessful; }
        }

        public bool IsNotOk
        {
            get { return !IsOk; }
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as Result;

            if (ReferenceEquals(this, otherObj))
            {
                return true;
            }

            if (otherObj == null)
            {
                return false;
            }

            return Code == otherObj.Code && Message == otherObj.Message;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}