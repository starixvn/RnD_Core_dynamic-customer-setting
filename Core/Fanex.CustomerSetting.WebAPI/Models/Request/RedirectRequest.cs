namespace Fanex.CustomerSetting.WebApi.Models.Request
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;

    public class RedirectRequest : SettingRequest
    {
        [Range(1, short.MaxValue)]
        public byte SettingTypeId { get; set; }

        [Range(0, 255)]
        public byte ActionId { get; set; }

        [Range(0, 2)]
        public byte ActionType { get; set; }

        public string JsonParameters { get; set; }

        internal RedirectCommand ToRouteCommand()
        {
            return new RedirectCommand
            {
                SettingTypeId = SettingTypeId,
                ActionId = ActionId,
                ActionType = (ActionType)ActionType,
                JsonParameters = JsonParameters
            };
        }
    }
}