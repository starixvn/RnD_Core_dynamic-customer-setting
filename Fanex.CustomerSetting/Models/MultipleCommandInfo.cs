namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class MultipleCommandInfo : CommandInfo
    {
        public MultipleCommandInfo()
        {
        }

        public MultipleCommandInfo(MultipleCommandInfo multipleCommandInfo)
            : base(multipleCommandInfo)
        {
            this.TargetCustIds = multipleCommandInfo.TargetCustIds;
        }

        public IEnumerable<int> TargetCustIds { get; set; }

        public CommandInfo Take()
        {
            if (TargetCustIds.Count() == 0)
            {
                return null;
            }

            var targetCustId = TargetCustIds.First();
            this.TargetCustIds = this.TargetCustIds.Skip(1);
            var commandInfo = new CommandInfo(this);
            commandInfo.TargetCustId = targetCustId;
            return commandInfo;
        }
    }
}