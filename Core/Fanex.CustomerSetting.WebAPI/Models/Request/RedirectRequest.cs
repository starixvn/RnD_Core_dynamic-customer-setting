namespace Fanex.CustomerSetting.WebApi.Models.Request
{
    using System.ComponentModel.DataAnnotations;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.WebApi.Configurations;

    public class RedirectRequest : QuerySettingRequest
    {
        [Range(1, short.MaxValue)]
        public byte SettingTypeId { get; set; }

#pragma warning disable S109 // Magic numbers should not be used
        [Range(0, 255)]
        public byte ActionId { get; set; }

        [Range(0, 2)]
        public byte ActionType { get; set; }
#pragma warning restore S109 // Magic numbers should not be used

        public string JsonParameters { get; set; }

        internal RedirectCommand ToRedirectCommand()
        {
            return new RedirectCommand(
                CustomerId,
                CustomerLevel,
                ProductId,
                UplineLevel,
                SettingTypeId,
                ActionId,
                (ActionType)ActionType,
                CustomerSettingApiConfiguration.Settings.SettingStatus,
                JsonParameters);
        }
    }
}