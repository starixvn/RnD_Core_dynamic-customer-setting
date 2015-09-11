namespace Fanex.CustomerSetting.WebApi.Models.Request
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;

    public abstract class SettingRequest
    {
        private SettingContext _settingContext;

        [Range(2, 4)]
        public byte? UplineLevel { get; set; }

        [Range(1, int.MaxValue)]
        public int ActorId { get; set; }

        [Required]
        public string ActorName { get; set; }

        [Range(2, 5)]
        public byte ActorLevel { get; set; }

        [Range(1, int.MaxValue)]
        public int SubAccountId { get; set; }

        [Range(1, 3)]
        public byte System { get; set; }

        public string Domain { get; set; }

        public string ClientIP { get; set; }

        public string Language { get; set; }

        internal ISettingContext GetSettingContext()
        {
            if (_settingContext == null)
            {
                var actor = new User
                {
                    Id = this.ActorId,
                    Name = this.ActorName,
                    Level = Enumeration.FromValue<Level>(this.ActorLevel),
                    SubAccountId = this.SubAccountId
                };

                _settingContext = new SettingContext
                {
                    Actor = actor,
                    Language = this.Language,
                    IP = this.ClientIP,
                    System = this.System,
                    Domain = this.Domain
                };
            }

            return _settingContext;
        }
    }
}