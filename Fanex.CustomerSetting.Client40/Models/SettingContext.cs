namespace Fanex.CustomerSetting.Client40.Models
{
    using Fanex.CustomerSetting.Client40.Configurations;

    public class SettingContext
    {
        public int ActorId { get; set; }

        public string ActorName { get; set; }

        public byte ActorLevel { get; set; }

        public int SubAccountId { get; set; }

        public string Language { get; set; }

        public string Domain { get; set; }

        public string IP { get; set; }

        public byte System { get; set; } = CustomerSettingConfiguration.Settings.System;
    }
}