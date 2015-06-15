namespace Fanex.CustomerSetting.Core.Results
{
    using System.Collections.Generic;

    public interface IResults
    {
        IEnumerable<IResult> ResultList { get; set; }

        string Name { get; set; }

        int Id { get; set; }

        bool IsSuccessful { get; }
    }
}