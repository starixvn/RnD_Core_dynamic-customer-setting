namespace Fanex.CustomerSetting.SettingType3.DataAccesses
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
        private static readonly Dictionary<byte, string> QueryCommissionSpKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperCommissionSelect" },
            { Level.Master.Value, "CustSetting_MasterCommissionSelect" },
            { Level.Agent.Value, "CustSetting_AgentCommissionSelect" },
            { Level.Member.Value, "CustSetting_MemberCommissionSelect" }
        };

        public IEnumerable<CommissionItemDto> GetCustomerCommissionItemsRawData(CustomerSettingQuery query)
        {
            var settingKey = QueryCommissionSpKeys[query.CustomerLevel.Value];

            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(query, Constants.SettingTypeId);
                return objectDb.Query<CommissionItemDto>(parameterValues);
            }
        }

        public IResult UpdateCustomerCommission(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string commissionXml)
        {
            var param = new Hashtable();
            param.Add("userid", settingContext.Actor.Id);
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("custid", customerId);
            param.Add("ip", settingContext.IP);
            param.Add("discount", commissionXml);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            return ExecuteNonQuery("CustSetting_DiscountUpd", param);
        }
    }
}