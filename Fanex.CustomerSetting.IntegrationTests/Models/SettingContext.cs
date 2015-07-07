namespace Fanex.CustomerSetting.IntegrationTests.Models
{
    using Fanex.CustomerSetting.Models;

    public class SettingContext : ISettingContext
    {
        public SettingContext(User actor, string language, string ip, byte system, string domain)
        {
            Actor = actor;
            Language = language;
            IP = ip;
            System = system;
            Domain = domain;
        }

        public User Actor { get; set; }

        public string Language { get; set; }

        public string IP { get; private set; }

        public byte System { get; private set; }

        public string Domain { get; private set; }
    }
}