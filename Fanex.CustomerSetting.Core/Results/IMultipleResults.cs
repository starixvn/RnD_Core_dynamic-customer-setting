namespace Fanex.CustomerSetting.Core.Results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IMultipleResults
    {
        List<IResults> ResultsList { get; set; }

        bool IsSuccessful { get; }
    }
}
