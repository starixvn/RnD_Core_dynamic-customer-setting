namespace Fanex.CustomerSetting.Core.Results
{
    using System.Collections.Generic;

    public interface IResults
    {
        IEnumerable<IResult> ResultList { get; }

        string Name { get; }

        int Id { get; }

        bool IsSuccessful { get; }
    }
}