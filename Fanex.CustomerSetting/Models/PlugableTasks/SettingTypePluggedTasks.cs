namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using System.Collections.Generic;

    internal class SettingTypePluggedTasks
    {
        public short? SettingTypeId { get; set; }

        public IEnumerable<string> TaskNames { get; set; }
    }
}