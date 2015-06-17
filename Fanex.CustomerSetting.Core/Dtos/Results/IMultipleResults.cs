namespace Fanex.CustomerSetting.Core.Dtos.Results
{
    using System.Collections.Generic;

    public interface IMultipleResults
    {
        IEnumerable<IResults> ResultsList { get; }

        bool IsSuccessful { get; }
    }
}