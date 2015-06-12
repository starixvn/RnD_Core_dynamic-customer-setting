namespace Fanex.CS.Core.Results.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    public class CommandResults : ICommandResults
    {
        public CommandResults()
        {
            Results = new List<ICommandResult>();
        }

        public List<ICommandResult> Results { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }

        public virtual bool IsSuccessful
        {
            get
            {
                return Results.Where(x => !x.IsSuccessful).FirstOrDefault() == null;
            }
        }
    }
}
