namespace Fanex.CustomerSetting.SettingType2.DataAccesses
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
        private static readonly Dictionary<byte, string> QueryBetLimitSpKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperBetLimitSelect" },
            { Level.Master.Value, "CustSetting_MasterBetLimitSelect" },
            { Level.Agent.Value, "CustSetting_AgentBetLimitSelect" },
            { Level.Member.Value, "CustSetting_MemberBetLimitSelect" }
        };

        public IEnumerable<BetLimitItemDto> GetCustomerBetLimitItemsRawData(CustomerSettingQuery query)
        {
            var settingKey = QueryBetLimitSpKeys[query.CustomerLevel.Value];

            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(query, Constants.SettingTypeId);
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
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("systemid", settingContext.System);
            param.Add("custid", customerId);
            param.Add("ip", settingContext.IP);
            param.Add("betlimit", betLimitXml);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            return ExecuteNonQuery("CustSetting_BetLimitUpd", param);
        }
    }
}