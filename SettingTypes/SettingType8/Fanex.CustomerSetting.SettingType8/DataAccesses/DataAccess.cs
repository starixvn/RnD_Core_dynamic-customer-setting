namespace Fanex.CustomerSetting.SettingType8.DataAccesses
{
    using System;
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
        public IEnumerable<StakeLevelGroupDto> GetCustomerStakeLevelGroups(CustomerSettingQuery query)
        {
            using (var objectDb = new ObjectDb("CustSetting_StakeLevelGroupSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "productid", query.ProductId }
                };

                return objectDb.Query<StakeLevelGroupDto>(parameterValues);
            }
        }

        public IEnumerable<StakeLevelDetailDto> GetCustomerStakeLevelGroupsDetails(
            CustomerSettingQuery query,
            short sportTypeId,
            out int numberOfDetailedItems,
            bool showAll = false)
        {
            using (var objectDb = new ObjectDb("CustSetting_StakeLevelGroupDetailSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "productid", query.ProductId },
                    { "sporttypeid", sportTypeId },
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
                { "custid", customerId },
                { "ip", settingContext.IP },
                { "stakelevel", stakeLevelGroupXml },
                { "msgtype", 0 },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            return ExecuteNonQuery("CustSetting_StakeLevelUpd", param);
        }
    }
}