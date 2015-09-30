namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;

    public interface IProductService
    {
        IEnumerable<SettingType> GetProductSettingTypes(byte productId, byte systemId);

        SettingType GetProductSettingType(byte productId, short settingTypeId, byte systemId);

        IEnumerable<Product> GetAllProducts();
    }
}