namespace Fanex.CustomerSetting.SettingType13.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using CustomerSetting.DataAccesses.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Data;
    using Dtos;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        public IEnumerable<ProductStatusDto> GetCustomerProductStatus(CustomerSettingQuery query)
        {
            using (var objectDb = new ObjectDb("Acc_CustSetting_DisableNewProductStatusSelect"))
            {
                return objectDb.Query<ProductStatusDto>(new
                {
                    CustId = query.CustomerId,
                    ProductId = query.ProductId,
                    SettingStatus= query.SettingStatus
                });
            }
        }

        public IResult UpdateCustomerProductStatus(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string productStatusXml)
        {
            var parameters = new Hashtable
            {
                { "userid", settingContext.Actor.Id },
                { "subaccid", settingContext.Actor.SubAccountId },
                { "custid", customerId },
                { "ip", settingContext.IP },
                { "systemid", settingContext.System },
                { "isdisabled", productStatusXml },
                { "errcode", 0 },
                { "errmsg", string.Empty },
                { "msgtype", 0 }
            };

            return ExecuteNonQuery("Acc_CustSetting_DisableNewProductStatusUpd", parameters);
        }
    }
}