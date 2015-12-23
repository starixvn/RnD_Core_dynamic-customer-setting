namespace Fanex.CustomerSetting.SettingType7.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using Constants;
    using CustomerSetting.DataAccesses.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.SettingType7.Dtos;
    using Data;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        private static readonly Dictionary<byte, string> QueryingChargedCommissionSpKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperChargedCommissionSelect" },
            { Level.Master.Value, "CustSetting_MasterChargedCommissionSelect" },
            { Level.Agent.Value, "CustSetting_AgentChargedCommissionSelect" },
            { Level.Member.Value, "CustSetting_MemberChargedCommissionSelect" }
        };

        public IEnumerable<ChargedCommissionDto> GetCustomerChargedCommissionRawData(CustomerSettingQuery query)
        {
            var settingKey = QueryingChargedCommissionSpKeys[query.CustomerLevel.Value];

            using (var objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(query, Constants.SettingTypeId);

                return objectDb.Query<ChargedCommissionDto>(parameterValues);
            }
        }

        public IResult UpdateCustomerChargedCommission(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string chargedCommissionXml)
        {
            var param = new Hashtable();

            param.Add("userid", settingContext.Actor.Id);
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("custid", customerId);
            param.Add("ip", settingContext.IP);
            param.Add("chargedcommission", chargedCommissionXml);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            return ExecuteNonQuery("CustSetting_ChargedCommissionUpd", param);
        }
    }
}