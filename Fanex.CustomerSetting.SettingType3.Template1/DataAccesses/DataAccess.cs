namespace Fanex.CustomerSetting.SettingType3.Template1.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Requests;
    using Fanex.CustomerSetting.Results;
    using Fanex.CustomerSetting.SettingType3.Template1.Constants;
    using Fanex.CustomerSetting.SettingType3.Template1.Dtos;
    using Fanex.Data;

    public class DataAccess : BaseDataAccess, IDataAccess
    {
        private static readonly Dictionary<byte, string> QueryCommissionSpKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperCommissionSelect" },
            { Level.Master.Value, "CustSetting_MasterCommissionSelect" },
            { Level.Agent.Value, "CustSetting_AgentCommissionSelect" },
            { Level.Member.Value, "CustSetting_MemberCommissionSelect" }
        };

        public IEnumerable<CommissionItemDto> GetCustomerCommissionItemsRawData(CustomerSettingQuery queryParam)
        {
            var settingKey = QueryCommissionSpKeys[queryParam.CustomerLevel.Value];

            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(queryParam, Constants.SettingTypeId);
                return objectDb.Query<CommissionItemDto>(parameterValues);
            }
        }

        public IResult UpdateCustomerCommission(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
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