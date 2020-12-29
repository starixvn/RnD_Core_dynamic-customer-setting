namespace Fanex.CustomerSetting.SettingType15.DataAccesses
{
    using CustomerSetting.Models.Requests;
    using Dtos;
    using Fanex.CustomerSetting.DataAccesses;
    using System.Collections.Generic;

    internal class DataAccess : IDataAccess
    {
        public IEnumerable<WinningShareDto> GetCustomerWinningShare(CustomerSettingQuery query)
        {
            using (var objectDb = ObjectDbFactory.ObjectDb("Acc_CustSetting_WinningShareCommissionSelect"))
            {
                return objectDb.Query<WinningShareDto>(new
                {
                    CustId = query.CustomerId,
                    ProductId = query.ProductId,
                    SettingStatus = query.SettingStatus
                });
            }
        }
    }
}