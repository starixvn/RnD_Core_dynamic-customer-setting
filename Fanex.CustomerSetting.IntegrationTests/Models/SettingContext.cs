namespace Fanex.CustomerSetting.IntegrationTests.Models
{
    using Fanex.CustomerSetting.Models;

    public class SettingContext : ISettingContext
    {
        public SettingContext(
            User actor, 
            string language = "en-US", 
            string ip = "192.168.1.1", 
            byte system = 1, 
            string domain = "www.domain.com")
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

        public byte System { get; set; }

        public string Domain { get; private set; }
    }
}