namespace Fanex.CustomerSetting.SettingType8.DataAccesses
{
    using CustomerSetting.DataAccesses.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Results;
    using Data;
    using Dtos;
    using Fanex.CustomerSetting.DataAccesses;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        public IEnumerable<StakeLevelGroupDto> GetCustomerStakeLevelGroups(int customerId, byte productId, byte settingStatus)
        {
            using (var objectDb = ObjectDbFactory.ObjectDb("CustSetting_StakeLevelGroupSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", customerId },
                    { "productid", productId },
                    { "settingstatus", settingStatus }
                };

                return objectDb.Query<StakeLevelGroupDto>(parameterValues);
            }
        }

        public IEnumerable<StakeLevelDetailDto> GetCustomerStakeLevelGroupsDetails(
            int customerId,
            short sportTypeId,
            out int numberOfDetailedItems,
#pragma warning disable S1006 // Method overrides should not change parameter defaults
            bool showAll = false)
#pragma warning restore S1006 // Method overrides should not change parameter defaults
        {
            using (var objectDb = ObjectDbFactory.ObjectDb("CustSetting_StakeLevelGroupDetailSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", customerId },
                    { "sporttype", sportTypeId },
                    { "noofrows", 0 },
                    { "showall", showAll }
                };
                var stakeLevelGroups = objectDb.Query<StakeLevelDetailDto>(parameterValues);

                numberOfDetailedItems = parameterValues.ContainsKey("noofrows") ? Convert.ToInt32(parameterValues["noofrows"]) : 0;

                return stakeLevelGroups;
            }
        }

        public IResult UpdateCustomerStakeLevelGroups(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string stakeLevelGroupXml)
        {
            var param = new Hashtable
            {
                { "userid", settingContext.Actor.Id },
                { "subaccid", settingContext.Actor.SubAccountId },
                { "systemid", settingContext.System },
                { "custid", customerId },
                { "ip", settingContext.IP },
                { "stakelevel", stakeLevelGroupXml },
                { "msgtype", 0 },
                { "settingType", Constants.Constants.SettingTypeId },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            return ExecuteNonQuery("CustSetting_StakeLevelGroupUpd", param);
        }
    }
}