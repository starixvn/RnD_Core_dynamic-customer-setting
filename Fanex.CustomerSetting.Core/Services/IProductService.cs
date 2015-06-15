namespace Fanex.CustomerSetting.Core.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Models;

    public interface IProductService
    {
        Product GetProduct(byte productId);

        IEnumerable<SettingType> GetProductSettingTypes(byte productId);
    }
}