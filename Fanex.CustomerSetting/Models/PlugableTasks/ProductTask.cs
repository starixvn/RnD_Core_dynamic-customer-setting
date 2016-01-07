namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using System.Collections.Generic;

    internal class ProductTask
    {
        public byte? ProductId { get; set; }

        public IEnumerable<SettingTypeTask> Tasks { get; set; }
    }
}