namespace Fanex.CustomerSetting.Tasks.Product18.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.DataAccesses;
    using Models;

    public class DataAccess : IDataAccess
    {
        public IEnumerable<ProductsInfo> GetProducts(int currency, int custId, int settingStatus, string site)
        {
            using (var objectDb = ObjectDbFactory.ObjectDb("Acc_CustSetting_CheckMacauEnable"))
            {
                return objectDb.Query<ProductsInfo>(new
                {
                    currency,
                    custid = custId,
                    SettingStatus = settingStatus,
                    site
                });
            }
        }
    }
}