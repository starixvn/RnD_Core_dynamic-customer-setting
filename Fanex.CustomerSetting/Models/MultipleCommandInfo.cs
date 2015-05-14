namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Utility;

    public class MultipleCommandInfo : CommandInfo
    {
        public IEnumerable<int> TargetCustIds { get; set; }

        public CommandInfo Take()
        {
            if (TargetCustIds.Count() == 0)
            {
                return null;
            }

            var targetCustId = TargetCustIds.First();
            this.TargetCustIds = this.TargetCustIds.Skip(1);
            var commandInfo = this.Clone<CommandInfo>();
            commandInfo.TargetCustId = targetCustId;
            return commandInfo;
        }
    }
}