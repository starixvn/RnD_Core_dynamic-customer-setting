namespace Fanex.CustomerSetting.Core.Dtos.Results
{
    using System.Collections.Generic;

    public interface IResult
    {
        int Code { get; }

        int Id { get; }

        string Name { get; }

        string Message { get; }

        int MsgType { get; }

        bool IsSuccessful { get; }

        IEnumerable<IResult> SubResults { get; }

        IEnumerable<string> SubMessages { get; }
    }
}