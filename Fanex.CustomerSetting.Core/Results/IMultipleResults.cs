namespace Fanex.CustomerSetting.Core.Results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IMultipleResults
    {
        IEnumerable<IResults> ResultsList { get; }

        bool IsSuccessful { get; }
    }
}
