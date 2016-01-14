namespace Fanex.CustomerSetting.SettingType5.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType5.Constants;
    using Fanex.CustomerSetting.SettingType5.Dtos;
    using Fanex.Data;

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
            var param = new Hashtable
            {
                { "userid", settingContext.Actor.Id },
                { "subaccid", settingContext.Actor.SubAccountId },
                { "custid", customerId },
                { "settingtype", Constants.SettingTypeId },
                { "ip", settingContext.IP },
                { "discount", chargeCommissionXml },
                { "msgtype", 0 },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            return ExecuteNonQuery("CustSetting_ChargeCommissionUpd", param);
        }
    }
}