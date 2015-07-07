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
            var settingKey = GetSettingKeyForGettingSettingType(
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

        private string GetSettingKeyForGettingSettingType(
            CustomerLevel customerLevel,
            byte settingTypeId,
            byte settingTemplateId,
            byte productId)
        {
            var keySuffix = GetProductPrefixIncaseOfProductDoesNotUseDefaultSettingTypeSps(settingTypeId, settingTemplateId, productId);

            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    return "CustSetting_SuperBetLimitSelect" + keySuffix;

                case CustomerLevel.Master:
                    return "CustSetting_MasterBetLimitSelect" + keySuffix;

                case CustomerLevel.Agent:
                    return "CustSetting_AgentBetLimitSelect" + keySuffix;

                case CustomerLevel.Member:
                    return "CustSetting_MemberBetLimitSelect" + keySuffix;

                default:
                    throw new InvalidOperationException("This customer level is not supported");
            }
        }

        public IResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            CustomerInfo customer,
            SettingType settingType,
            byte productId,
            CustomerLevel? uplineLevel)
        {
            var param = new Hashtable();
            param.Add("userid", settingContext.Actor.Id);
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("custid", customer.CustomerId);
            param.Add("ip", settingContext.IP);
            param.Add("betlimit", settingType.ToXml());
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            var updateSettingKey = "CustSetting_BetLimitUpd";
            var keySuffix = GetProductPrefixIncaseOfProductDoesNotUseDefaultSettingTypeSps(
                BetLimitConstants.SettingTypeId, 
                BetLimitConstants.SettingTempateId, 
                productId);

            return ExecuteNonQuery(updateSettingKey + keySuffix, param);
        }
    }
}