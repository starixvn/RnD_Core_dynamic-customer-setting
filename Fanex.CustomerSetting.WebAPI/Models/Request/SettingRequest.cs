namespace Fanex.CustomerSetting.WebAPI.Models.Request
{
    using System.ComponentModel.DataAnnotations;

    public abstract class SettingRequest
    {
        [Range(1, 255)]
        public byte ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

        [Range(1, 4)]
        public byte CustomerLevel { get; set; }

        [Range(2, 4)]
        public byte? UplineLevel { get; set; }

        [Range(1, int.MaxValue)]
        public int ActorId { get; set; }

        [Required]
        public string ActorName { get; set; }

        [Range(2, 5)]
        public byte ActorLevel { get; set; }

        public int SubAccountId { get; set; }

        [Range(1, 3)]
        public byte SystemId { get; set; }

        public string Domain { get; set; }

        public string ClientIp { get; set; }

        public string Language { get; set; }
    }
}