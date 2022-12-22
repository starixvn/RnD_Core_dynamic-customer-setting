namespace Fanex.CustomerSetting.Tasks.Product18.DataAccesses
{
    using System.Collections.Generic;
    using Models;

    public interface IDataAccess
    {
        IEnumerable<ProductsInfo> GetProducts(int currency, int custId, int settingStatus, string site);
    }
}