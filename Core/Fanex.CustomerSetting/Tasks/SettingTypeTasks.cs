namespace Fanex.CustomerSetting.Tasks
{
    using System.Collections.Generic;

    internal class SettingTypeTasks
    {
        public short? SettingTypeId { get; set; }

        public IEnumerable<string> TaskNames { get; set; }
    }
}