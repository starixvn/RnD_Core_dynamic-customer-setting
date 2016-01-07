namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using System;
    using System.Collections.Generic;

    internal class SettingTypeTask
    {
        public short? SettingTypeId { get; set; }

        public IEnumerable<Type> PreTasks { get; set; }

        public IEnumerable<Type> PostTasks { get; set; }
    }
}