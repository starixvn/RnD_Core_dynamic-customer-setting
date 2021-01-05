namespace Fanex.CustomerSetting.SettingType9.DataAccesses
{
    using CustomerSetting.DataAccesses.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Dtos;
    using Fanex.CustomerSetting.DataAccesses;
    using System.Collections;
    using System.Collections.Generic;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        public IEnumerable<BetLimitDto> GetCustomerBetLimit(CustomerSettingQuery query)
        {
            using (var objectDb = ObjectDbFactory.ObjectDb("Acc_CustSetting_GameBetLimitSelect"))
            {
                return objectDb.Query<BetLimitDto>(new
                {
                    CustId = query.CustomerId,
                    ProductId = query.ProductId,
                    SettingStatus = query.SettingStatus
                });
            }
        }

        public IResult UpdateCustomerBetLimit(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string betLimitXml)
        {
            var parameters = new Hashtable
            {
                { "userid", settingContext.Actor.Id },
                { "subaccid", settingContext.Actor.SubAccountId },
                { "systemid", settingContext.System },
                { "custid", customerId },
                { "ip", settingContext.IP },
                { "gamebetlimit", betLimitXml },
                { "errcode", 0 },
                { "errmsg", string.Empty },
                { "msgtype", 0 }
            };

            return ExecuteNonQuery("Acc_CustSetting_GameBetLimitUpd", parameters);
        }
    }
}