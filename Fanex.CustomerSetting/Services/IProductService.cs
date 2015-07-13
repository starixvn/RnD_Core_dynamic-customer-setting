namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;

    public interface IProductService
    {
        IEnumerable<SettingType> GetProductSettingTypes(byte productId);

        SettingType GetProductSettingType(byte productId, byte settingTypeId);
    }
}