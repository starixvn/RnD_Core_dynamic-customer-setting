namespace Fanex.CustomerSetting.Tasks
{
    using System.Collections.Generic;

    internal class SiteProductTasks
    {
        public byte? System { get; set; }

        public byte? ProductId { get; set; }

        public IEnumerable<SettingTypeTasks> Tasks { get; set; }
    }
}