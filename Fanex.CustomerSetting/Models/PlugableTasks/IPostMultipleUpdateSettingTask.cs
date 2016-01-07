namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using System.Collections.Generic;
    using Requests;
    using Results;

    public interface IPostMultipleUpdateSettingTask
    {
        void Invoke(MultipleUpdateCustomerSettingCommand command, ref IEnumerable<SettingResult> settingResults);
    }
}