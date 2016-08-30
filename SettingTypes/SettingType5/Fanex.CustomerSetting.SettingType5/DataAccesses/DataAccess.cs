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
    using Data;
    using Dtos;

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
                var parameters = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "productid", query.ProductId }
                };

                if (query.CustomerLevel == Level.Member)
                {
                    parameters.Add("roleid", query.UplineLevel.Value);
                }

                return objectDb.Query<ChargeCommissionDto>(parameters);
            }
        }

        public IResult UpdateCustomerChargeCommission(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string chargeCommissionXml)
        {
            var parameters = new Hashtable
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

            return ExecuteNonQuery("CustSetting_ChargeCommissionUpd", parameters);
        }
    }
}