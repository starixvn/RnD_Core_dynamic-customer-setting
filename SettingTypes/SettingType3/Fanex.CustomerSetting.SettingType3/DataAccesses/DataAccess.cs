namespace Fanex.CustomerSetting.SettingType3.DataAccesses
{
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
                var parameters = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "productid", query.ProductId }
                };

                if (query.CustomerLevel == Level.Member)
                {
                    parameters.Add("uplineroleid", query.UplineLevel.Value);
                }

                return objectDb.Query<CommissionItemDto>(parameters);
            }
        }

        public IResult UpdateCustomerCommission(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string commissionXml,
            int settingTypeId)
        {
            var parameters = new Hashtable
            {
                { "userid", settingContext.Actor.Id },
                { "subaccid", settingContext.Actor.SubAccountId },
                { "custid", customerId },
                { "ip", settingContext.IP },
                { "discount", commissionXml },
                { "settingType", settingTypeId },
                { "msgtype", 0 },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            return ExecuteNonQuery("CustSetting_DiscountUpd", parameters);
        }
    }
}