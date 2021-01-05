namespace Fanex.CustomerSetting.Client40.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
    public abstract class SettingRequest
#pragma warning restore S1694 // An abstract class should have both abstract and concrete methods
    {
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
#pragma warning restore S109 // Magic numbers should not be used

        public string Domain { get; set; }

        public string Language { get; set; }

        public string ClientIP { get; set; }

        public byte System { get; set; }
    }
}