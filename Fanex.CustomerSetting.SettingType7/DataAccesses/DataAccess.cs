namespace Fanex.CustomerSetting.SettingType7.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType7.Constants;
    using Fanex.CustomerSetting.SettingType7.Dtos;
    using Fanex.Data;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        private static readonly Dictionary<byte, string> QueryingStakeLevelSpKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperStakeLevelSelect" },
            { Level.Master.Value, "CustSetting_MasterStakeLevelSelect" },
            { Level.Agent.Value, "CustSetting_AgentStakeLevelSelect" },
            { Level.Member.Value, "CustSetting_MemberStakeLevelSelect" }
        };

        public IEnumerable<StakeLevelDto> GetCustomerStakeLevelRawData(CustomerSettingQuery query)
        {
            string settingKey = QueryingStakeLevelSpKeys[query.CustomerLevel.Value];

            using (var objectDb = new ObjectDb(settingKey))
            {
                Hashtable parameterValues = GetSPParamsForGettingSettingType(query, Constants.SettingTypeId);

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