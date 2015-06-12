namespace Fanex.CS.Core.Results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IMultipleCommandResults
    {
        List<ICommandResults> CommandResultsList { get; set; }

        bool IsSuccessful { get; }
    }
}
