namespace Fanex.CustomerSetting.SettingType23.DataAccesses
{
    using Constants;
    using CustomerSetting.DataAccesses.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Dtos;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.DataAccesses;
    using System.Collections;
    using System.Collections.Generic;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        public IEnumerable<BetLimitItemDto> GetCustomerBetLimitItemsRawData(CustomerSettingQuery query)
        {
            using (var objectDb = ObjectDbFactory.ObjectDb("Acc_CustSetting_BetSetting23Select"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "productid", query.ProductId },
                    { "settingstatus", query.SettingStatus }
                };

                return objectDb.Query<BetLimitItemDto>(parameterValues);
            }
        }

        public IResult UpdateCustomerBetLimit(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string betLimitXml)
        {
            var param = new Hashtable();
            param.Add("userid", settingContext.Actor.Id);
            param.Add("custid", customerId);
            param.Add("ip", settingContext.IP);
            param.Add("betlimit", betLimitXml);
            param.Add("settingstatus", AppSettings.SettingStatus);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            return ExecuteNonQuery("Acc_CustSetting_BetSetting23Upd", param);
        }
    }
}