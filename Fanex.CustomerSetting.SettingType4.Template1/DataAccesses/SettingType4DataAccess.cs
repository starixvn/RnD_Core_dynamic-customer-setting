namespace Fanex.CustomerSetting.SettingType4.Template1.DataAccesses
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
    using Fanex.CustomerSetting.SettingType4.Template1.Constants;
    using Fanex.CustomerSetting.SettingType4.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType4.Template1.Models;
    using Fanex.Data;

    public class SettingType4DataAccess : BaseDataAccess, ISettingTypeDataAccess
    {
        public SettingType GetCustomerSettingType(
            User actor,
            QuerySettingParam queryParam)
        {
            var productStatusItemDtoList = ReadProductStatusItemsRawData(queryParam);

            return new SettingType
            {
                Name = Label.ProductStatus,
                SettingItems = productStatusItemDtoList.Select<ProductStatusItemDto, ProductStatusItem>(productStatusItemDto => new ProductStatusItem(productStatusItemDto))
            };
        }

        private IEnumerable<ProductStatusItemDto> ReadProductStatusItemsRawData(QuerySettingParam queryParam)
        {
            var settingKey = GetSettingKeyForGettingSettingType(
                queryParam.CustomerLevel, 
                ProductStatusConstants.SettingTypeId, 
                ProductStatusConstants.SettingTempateId,
                queryParam.ProductId);
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(queryParam, ProductStatusConstants.SettingTypeId);
                return objectDb.Query<ProductStatusItemDto>(parameterValues);
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
                    return "CustSetting_SuperProductStatusSelect" + keySuffix;

                case CustomerLevel.Master:
                    return "CustSetting_MasterProductStatusSelect" + keySuffix;

                case CustomerLevel.Agent:
                    return "CustSetting_AgentProductStatusSelect" + keySuffix;

                case CustomerLevel.Member:
                    return "CustSetting_MemberProductStatusSelect" + keySuffix;

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
            param.Add("isdisabled", settingType.ToXml());
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            var updateSettingKey = "CustSetting_DisableStatUpd";
            var keySuffix = GetProductPrefixIncaseOfProductDoesNotUseDefaultSettingTypeSps(
                ProductStatusConstants.SettingTypeId,
                ProductStatusConstants.SettingTempateId,
                productId);

            return ExecuteNonQuery(updateSettingKey + keySuffix, param);
        }
    }
}