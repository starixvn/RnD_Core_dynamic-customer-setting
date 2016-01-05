namespace Fanex.CustomerSetting.SettingType5.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using Constants;
    using CustomerSetting.DataAccesses.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.SettingType5.Dtos;
    using Data;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        private static readonly Dictionary<byte, string> QueryingChargeCommissionSpKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperChargeCommissionSelect" },
            { Level.Master.Value, "CustSetting_MasterChargeCommissionSelect" },
            { Level.Agent.Value, "CustSetting_AgentChargeCommissionSelect" },
            { Level.Member.Value, "CustSetting_MemberChargeCommissionSelect" }
        };

        public IEnumerable<ChargeCommissionDto> GetCustomerChargeCommissionRawData(CustomerSettingQuery query)
        {
            string settingKey = QueryingChargeCommissionSpKeys[query.CustomerLevel.Value];

            using (var objectDb = new ObjectDb(settingKey))
            {
                Hashtable parameterValues = GetSPParamsForGettingSettingType(query, Constants.SettingTypeId);

                return objectDb.Query<ChargeCommissionDto>(parameterValues);
            }
        }

        public IResult UpdateCustomerChargeCommission(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string chargeCommissionXml)
        {
            var param = new Hashtable();

            param.Add("userid", settingContext.Actor.Id);
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("custid", customerId);
            param.Add("settingtype", Constants.SettingTypeId);
            param.Add("ip", settingContext.IP);
            param.Add("discount", chargeCommissionXml);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            return ExecuteNonQuery("CustSetting_ChargeCommissionUpd", param);
        }
    }
}