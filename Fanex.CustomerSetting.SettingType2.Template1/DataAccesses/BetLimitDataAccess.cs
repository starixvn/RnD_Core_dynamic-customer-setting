namespace Fanex.CustomerSetting.SettingType2.Template1.DataAccesses
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
    using Fanex.CustomerSetting.SettingType2.Template1.Constants;
    using Fanex.CustomerSetting.SettingType2.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType2.Template1.Models;
    using Fanex.Data;

    public class BetLimitDataAccess : BaseDataAccess, IBetLimitDataAccess
    {
        private static readonly Dictionary<byte, string> QueryBetLimitSpKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperBetLimitSelect" },
            { Level.Master.Value, "CustSetting_MasterBetLimitSelect" },
            { Level.Agent.Value, "CustSetting_AgentBetLimitSelect" },
            { Level.Member.Value, "CustSetting_MemberBetLimitSelect" }
        };

        public SettingType GetCustomerBetLimit(QuerySettingParam queryParam)
        {
            var betLimitItemDtoList = ReadBetLimitItemsRawData(queryParam);

            return new SettingType
            {
                Name = Label.BetSetting,
                SettingItems = betLimitItemDtoList.Select<BetLimitItemDto, BetLimitItem>(betLimitDto => new BetLimitItem(betLimitDto))
            };
        }

        private IEnumerable<BetLimitItemDto> ReadBetLimitItemsRawData(QuerySettingParam queryParam)
        {
            var settingKey = CreateQueryBetLimitSPSettingKey(
                queryParam.CustomerLevel,
                BetLimitConstants.SettingTypeId,
                BetLimitConstants.SettingTempateId,
                queryParam.ProductId);

            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(queryParam, BetLimitConstants.SettingTypeId);
                return objectDb.Query<BetLimitItemDto>(parameterValues);
            }
        }

        private string CreateQueryBetLimitSPSettingKey(
            Level customerLevel,
            byte settingTypeId,
            byte settingTemplateId,
            byte productId)
        {
            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(settingTypeId, settingTemplateId, productId);

            return QueryBetLimitSpKeys[customerLevel.Value] + keySuffix;
        }

        public IResult UpdateCustomerBetLimit(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            string betLimitXml)
        {
            var param = new Hashtable();
            param.Add("userid", settingContext.Actor.Id);
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("custid", customerId);
            param.Add("ip", settingContext.IP);
            param.Add("betlimit", betLimitXml);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            var updateSettingKey = "CustSetting_BetLimitUpd";

            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(
                BetLimitConstants.SettingTypeId,
                BetLimitConstants.SettingTempateId,
                productId);

            return ExecuteNonQuery(updateSettingKey + keySuffix, param);
        }
    }
}