namespace Fanex.CustomerSetting.SettingType8.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using CustomerSetting.DataAccesses.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Results;
    using Data;
    using Dtos;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        public IEnumerable<StakeLevelGroupDto> GetCustomerStakeLevelGroups(int customerId, byte productId)
        {
            using (var objectDb = new ObjectDb("CustSetting_StakeLevelGroupSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", customerId },
                    { "productid", productId }
                };

                return objectDb.Query<StakeLevelGroupDto>(parameterValues);
            }
        }

        public IEnumerable<StakeLevelDetailDto> GetCustomerStakeLevelGroupsDetails(
            int customerId,
            short sportTypeId,
            out int numberOfDetailedItems,
            bool showAll = false)
        {
            using (var objectDb = new ObjectDb("CustSetting_StakeLevelGroupDetailSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", customerId },
                    { "sporttype", sportTypeId },
                    { "noofrows", 0 },
                    { "showall", showAll }
                };
                var stakeLevelGroups = objectDb.Query<StakeLevelDetailDto>(parameterValues);

                if (parameterValues.ContainsKey("noofrows"))
                {
                    numberOfDetailedItems = Convert.ToInt32(parameterValues["noofrows"]);
                }
                else
                {
                    numberOfDetailedItems = 0;
                }

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