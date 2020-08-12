namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models.Results;

    public interface IUMService
    {
        SettingResult GetUMResult(int custId, byte productId, int? settingTypeId = null, string language = "en-US");
    }
}
