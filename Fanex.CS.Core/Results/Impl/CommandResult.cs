namespace Fanex.CS.Core.Results.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    public class CommandResult : CommandResultBase
    {
        public override bool IsSuccessful
        {
            get { return this.Code == 0; }
        }
    }
}
