namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;

    public class CommandResult : Result
    {
        public CommandResult(CommandInfo commandInfo, Result result)
            : base(result)
        {
            CommandType = commandInfo.CommandType;
            CustId = commandInfo.CustId;
        }

        public CommandResult(CommandInfo commandInfo, SettingResult settingResult)
            : this(commandInfo, new List<SettingResult> { settingResult })
        {
        }

        public CommandResult(CommandInfo commandInfo, IList<SettingResult> settingResults)
            : this(commandInfo, Result.Success)
        {
            SettingResults = settingResults;
        }

        public int CustId { get; private set; }

        public CommandType CommandType { get; private set; }

        public IList<SettingResult> SettingResults { get; private set; }

        public SettingResult this[SettingType settingType]
        {
            get
            {
                foreach (var settingResult in SettingResults)
                {
                    if (settingResult.SettingType == settingType)
                    {
                        return settingResult;
                    }
                }

                throw new KeyNotFoundException();
            }
        }
    }
}