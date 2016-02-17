namespace Fanex.CustomerSetting.Client40.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    public class RedirectRequest
    {
        [Range(1, short.MaxValue)]
        public short SettingTypeId { get; set; }

        [Range(0, byte.MaxValue)]
        public byte ActionId { get; set; }

        [Range(0, 2)]
        public byte ActionType { get; set; }

        public dynamic Parameters { get; set; }
    }
}