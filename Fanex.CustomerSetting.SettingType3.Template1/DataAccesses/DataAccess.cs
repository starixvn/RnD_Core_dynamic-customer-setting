namespace Fanex.CustomerSetting.SettingType3.Template1.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.SettingType3.Template1.Constants;
    using Fanex.CustomerSetting.SettingType3.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType3.Template1.Models;
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

        public IEnumerable<CommissionItemDto> GetCustomerCommissionItemsRawData(QuerySettingParam queryParam)
        {
            var settingKey = CreateQueryCommissionSPSettingKey(
                queryParam.CustomerLevel,
                Constants.SettingTypeId,
                Constants.SettingTempateId,
                queryParam.ProductId);

            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(queryParam, Constants.SettingTypeId);
                return objectDb.Query<CommissionItemDto>(parameterValues);
            }
        }

        private string CreateQueryCommissionSPSettingKey(
            Level customerLevel,
            byte settingTypeId,
            byte settingTemplateId,
            byte productId)
        {
            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(settingTypeId, settingTemplateId, productId);

            return QueryCommissionSpKeys[customerLevel.Value] + keySuffix;
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

            var updateSettingKey = "CustSetting_DiscountUpd";
            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(
                Constants.SettingTypeId,
                Constants.SettingTempateId,
                productId);

            return ExecuteNonQuery(updateSettingKey + keySuffix, param);
        }
    }
}