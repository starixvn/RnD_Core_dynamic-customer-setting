namespace Fanex.CS.Core.Results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    public interface ICommandResults
    {
        List<ICommandResult> Results { get; set; }

        string Name { get; set; }

        int Id { get; set; }

        bool IsSuccessful { get; }
    }
}
