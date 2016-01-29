namespace Fanex.CustomerSetting.Tasks
{
    using System.Collections.Generic;

    internal class SiteProductPluggedTasks
    {
        public byte? System { get; set; }

        public byte? ProductId { get; set; }

        public IEnumerable<SettingTypePluggedTasks> Tasks { get; set; }
    }
}