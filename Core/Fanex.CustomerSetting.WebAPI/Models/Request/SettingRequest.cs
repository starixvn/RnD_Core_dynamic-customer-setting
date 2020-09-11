namespace Fanex.CustomerSetting.WebApi.Models.Request
{
    using System.ComponentModel.DataAnnotations;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;

#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
    public abstract class SettingRequest
#pragma warning restore S1694 // An abstract class should have both abstract and concrete methods
    {
        private SettingContext _settingContext;

#pragma warning disable S109 // Magic numbers should not be used
        [Range(2, 4)]
        public byte? UplineLevel { get; set; }

        [Range(1, int.MaxValue)]
        public int ActorId { get; set; }

        [Required]
        public string ActorName { get; set; }

        [Range(2, 5)]
        public byte ActorLevel { get; set; }

        [Range(0, int.MaxValue)]
        public int SubAccountId { get; set; }

        [Range(1, 9)]
        public byte System { get; set; }
#pragma warning restore S109 // Magic numbers should not be used

        public string Domain { get; set; }

        public string ClientIP { get; set; }

        public string Language { get; set; }

        internal ISettingContext GetSettingContext()
        {
            if (_settingContext == null)
            {
                var actor = new User
                {
                    Id = ActorId,
                    Name = ActorName,
                    Level = Enumeration.FromValue<Level>(ActorLevel),
                    SubAccountId = SubAccountId
                };

                _settingContext = new SettingContext
                {
                    Actor = actor,
                    Language = Language,
                    IP = ClientIP,
                    System = System,
                    Domain = Domain
                };
            }

            return _settingContext;
        }
    }
}