namespace Fanex.CustomerSetting.Core.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Models;

    public interface IProductService
    {
        IEnumerable<SettingType> GetProductSettingTypes(byte productId);
    }
}