namespace Fanex.CustomerSetting.SettingType11.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using Constants;
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
            using (var objectDb = new ObjectDb("CustSetting_DisableStatSelect"))
            {
                var parameters = GetSPParamsForGettingSettingType(query, Constants.SettingTypeId);

                return objectDb.Query<ProductStatusDto>(parameters);
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
                { "isdisabled", productStatusXml },
                { "errcode", 0 },
                { "errmsg", string.Empty },
                { "msgtype", 0 }
            };

            return ExecuteNonQuery("CustSetting_DisableStatUpd", parameters);
        }
    }
}