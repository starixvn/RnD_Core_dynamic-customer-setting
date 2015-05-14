namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;

    public class CommandResult : Result
    {
        public CommandResult(CommandInfo commandInfo, SettingResult settingResult)
            : this(commandInfo)
        {
            SettingResults[settingResult.SettingType] = settingResult;
        }

        public CommandResult(CommandInfo commandInfo, IEnumerable<SettingResult> settingResults)
            : this(commandInfo)
        {
            foreach (var settingResult in settingResults)
            {
                SettingResults[settingResult.SettingType] = settingResult;
            }
        }

        protected CommandResult(CommandInfo commandInfo)
            : base(Result.Success)
        {
            CommandType = commandInfo.CommandType;
            CustId = commandInfo.CustId;
            SettingResults = new Dictionary<SettingType, SettingResult>();
        }

        public CommandResult(CommandInfo commandInfo, Result result)
            : base(result)
        {
            CommandType = commandInfo.CommandType;
            CustId = commandInfo.CustId;
        }

        public int CustId { get; private set; }

        public CommandType CommandType { get; private set; }

        public IDictionary<SettingType, SettingResult> SettingResults { get; private set; }
    }
}