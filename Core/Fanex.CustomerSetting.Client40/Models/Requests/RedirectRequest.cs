namespace Fanex.CustomerSetting.Client40.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    public class RedirectRequest : QuerySettingRequest
    {
        [Range(1, short.MaxValue)]
        public short SettingTypeId { get; set; }

        [Range(0, byte.MaxValue)]
        public byte ActionId { get; set; }

#pragma warning disable S109 // Magic numbers should not be used
        [Range(0, 2)]
#pragma warning restore S109 // Magic numbers should not be used
        public byte ActionType { get; set; }

        public string JsonParameters { get; set; }
    }
}