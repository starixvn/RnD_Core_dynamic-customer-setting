namespace Fanex.CustomerSetting.SettingType7.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType7.Dtos;
    using Fanex.Data;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        public IEnumerable<StakeLevelDto> GetCustomerStakeLevelRawData(CustomerSettingQuery query)
        {
            using (var objectDb = new ObjectDb("CustSetting_StakeLevelSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "productid", query.ProductId }
                };

                return objectDb.Query<StakeLevelDto>(parameterValues);
            }
        }

        public IResult UpdateCustomerStakeLevel(ISettingContext settingContext, int customerId, Level customerLevel, Level uplineLevel, string stakeLevelXml)
        {
            var param = new Hashtable
            {
                { "userid", settingContext.Actor.Id },
                { "subaccid", settingContext.Actor.SubAccountId },
                { "custid", customerId },
                { "ip", settingContext.IP },
                { "stakelevel", stakeLevelXml },
                { "msgtype", 0 },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            return ExecuteNonQuery("CustSetting_StakeLevelUpd", param);
        }
    }
}