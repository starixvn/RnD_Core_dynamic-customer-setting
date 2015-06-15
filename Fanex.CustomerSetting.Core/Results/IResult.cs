namespace Fanex.CustomerSetting.Core.Results
{
    using System.Collections.Generic;

    public interface IResult
    {
        int ErrCode { get; }

        string ErrMsg { get; }

        bool IsSuccessful { get; }

        IEnumerable<string> SubMessages { get; }
    }
}