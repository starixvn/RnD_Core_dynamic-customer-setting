namespace Fanex.CustomerSetting.SettingType4.Template1.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Requests;
    using Fanex.CustomerSetting.Results;
    using Fanex.CustomerSetting.SettingType4.Template1.Constants;
    using Fanex.CustomerSetting.SettingType4.Template1.Dtos;
    using Fanex.Data;

    public class DataAccess : BaseDataAccess, IDataAccess
    {
        private static readonly Dictionary<byte, string> QueryProductStatusSpKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperProductStatusSelect" },
            { Level.Master.Value, "CustSetting_MasterProductStatusSelect" },
            { Level.Agent.Value, "CustSetting_AgentProductStatusSelect" },
            { Level.Member.Value, "CustSetting_MemberProductStatusSelect" }
        };

        public IEnumerable<ProductStatusItemDto> GetCustomerProductStatusItemsRawData(CustomerSettingQuery queryParam)
        {
            var settingKey = QueryProductStatusSpKeys[queryParam.CustomerLevel.Value];

            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(queryParam, Constants.SettingTypeId);
                return objectDb.Query<ProductStatusItemDto>(parameterValues);
            }
        }

        public IResult UpdateCustomerProductStatus(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            string productStatusXml)
        {
            var param = new Hashtable();
            param.Add("userid", settingContext.Actor.Id);
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("custid", customerId);
            param.Add("ip", settingContext.IP);
            param.Add("isdisabled", productStatusXml);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            return ExecuteNonQuery("CustSetting_DisableStatUpd", param);
        }
    }
}