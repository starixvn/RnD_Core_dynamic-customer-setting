namespace Fanex.CustomerSetting.Tasks
{
    using System.Collections.Generic;

    internal class SettingTypePluggedTasks
    {
        public short? SettingTypeId { get; set; }

        public IEnumerable<string> TaskNames { get; set; }
    }
}