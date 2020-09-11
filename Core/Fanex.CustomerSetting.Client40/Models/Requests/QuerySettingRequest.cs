namespace Fanex.CustomerSetting.Client40.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    public class QuerySettingRequest : SettingRequest
    {
#pragma warning disable S109 // Magic numbers should not be used
        [Range(1, 255)]
#pragma warning restore S109 // Magic numbers should not be used
        public byte ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

#pragma warning disable S109 // Magic numbers should not be used
        [Range(1, 4)]
#pragma warning restore S109 // Magic numbers should not be used
        public byte CustomerLevel { get; set; }
    }
}