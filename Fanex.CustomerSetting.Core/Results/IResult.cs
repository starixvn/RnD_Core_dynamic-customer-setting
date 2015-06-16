namespace Fanex.CustomerSetting.Core.Results
{
    using System.Collections.Generic;

    public interface IResult
    {
        int Code { get; }

        string Message { get; }

        int MsgType { get; }

        bool IsSuccessful { get; }

        IEnumerable<string> SubMessages { get; }
    }
}