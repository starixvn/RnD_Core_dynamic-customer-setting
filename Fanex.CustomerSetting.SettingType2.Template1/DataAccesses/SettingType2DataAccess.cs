namespace Fanex.CustomerSetting.SettingType2.Template1.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.DataAccesses;
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

    public class SettingType2DataAccess : BaseDataAccess, ISettingTypeDataAccess
    {
        public SettingType GetCustomerSettingType(User actor, QuerySettingParam queryParam)
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
            var settingKey = GetSettingKeyForQuerying(
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

        private string GetSettingKeyForQuerying(
            UserLevel customerLevel,
            byte settingTypeId,
            byte settingTemplateId,
            byte productId)
        {
            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(settingTypeId, settingTemplateId, productId);

            switch (customerLevel)
            {
                case UserLevel.Super:
                    return "CustSetting_SuperBetLimitSelect" + keySuffix;

                case UserLevel.Master:
                    return "CustSetting_MasterBetLimitSelect" + keySuffix;

                case UserLevel.Agent:
                    return "CustSetting_AgentBetLimitSelect" + keySuffix;

                case UserLevel.Member:
                    return "CustSetting_MemberBetLimitSelect" + keySuffix;

                default:
                    throw new InvalidOperationException("This customer level is not supported");
            }
        }

        public IResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            UserLevel customerLevel,
            SettingType settingType,
            byte productId,
            UserLevel uplineLevel)
        {
            var param = new Hashtable();
            param.Add("userid", settingContext.Actor.Id);
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("custid", customerId);
            param.Add("ip", settingContext.IP);
            param.Add("betlimit", settingType.ToXml());
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