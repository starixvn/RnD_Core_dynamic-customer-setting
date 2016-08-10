namespace Fanex.CustomerSetting.SettingType9.DataAccesses
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
        public IEnumerable<BetLimitDto> GetCustomerBetLimit(CustomerSettingQuery query)
        {
            using (var objectDb = new ObjectDb("Acc_CustSetting_GameBetLimitSelect"))
            {
                var parameters = GetSPParamsForGettingSettingType(query, Constants.SettingTypeId);

                return objectDb.Query<BetLimitDto>(parameters);
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