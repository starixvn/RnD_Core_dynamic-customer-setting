namespace Fanex.CustomerSetting.SettingType7.DataAccesses
{
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
        public IEnumerable<StakeLevelDto> GetCustomerStakeLevelRawData(CustomerSettingQuery query)
        {
            using (var objectDb = ObjectDbFactory.ObjectDb("CustSetting_StakeLevelSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "productid", query.ProductId },
                    { "settingType", Constants.Constants.SettingTypeId },
                    { "settingstatus", query.SettingStatus }
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
                { "systemid", settingContext.System },
                { "custid", customerId },
                { "ip", settingContext.IP },
                { "stakelevel", stakeLevelXml },
                { "msgtype", 0 },
                { "settingType", Constants.Constants.SettingTypeId },
                { "settingstatus", AppSettings.SettingStatus },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            return ExecuteNonQuery("CustSetting_StakeLevelUpd", param);
        }
    }
}