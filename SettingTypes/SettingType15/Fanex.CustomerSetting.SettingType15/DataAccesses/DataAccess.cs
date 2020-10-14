namespace Fanex.CustomerSetting.SettingType15.DataAccesses
{
    using System.Collections.Generic;
    using CustomerSetting.Models.Requests;
    using Data;
    using Dtos;

    internal class DataAccess : IDataAccess
    {
        public IEnumerable<WinningShareDto> GetCustomerWinningShare(CustomerSettingQuery query)
        {
            using (var objectDb = new ObjectDb("Acc_CustSetting_WinningShareCommissionSelect"))
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