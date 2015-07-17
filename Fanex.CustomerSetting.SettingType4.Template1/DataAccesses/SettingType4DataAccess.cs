namespace Fanex.CustomerSetting.SettingType4.Template1.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
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
        private static readonly Dictionary<byte, string> QueryProductStatusSpKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperProductStatusSelect" },
            { Level.Master.Value, "CustSetting_MasterProductStatusSelect" },
            { Level.Agent.Value, "CustSetting_AgentProductStatusSelect" },
            { Level.Member.Value, "CustSetting_MemberProductStatusSelect" }
        };

        public SettingType GetCustomerSettingType(
            User actor,
            QuerySettingParam queryParam)
        {
            // Will bring this logic into service class later
            if (queryParam.UplineLevel != null
                && !queryParam.UplineLevel.IsDirectUplineLevelOf(queryParam.CustomerLevel))
            {
                return new SettingType();
            }

            var productStatusItemDtoList = ReadProductStatusItemsRawData(queryParam);

            return new SettingType
            {
                Name = Label.ProductStatus,
                SettingItems = productStatusItemDtoList.Select<ProductStatusItemDto, ProductStatusItem>(productStatusItemDto => new ProductStatusItem(productStatusItemDto))
            };
        }

        private IEnumerable<ProductStatusItemDto> ReadProductStatusItemsRawData(QuerySettingParam queryParam)
        {
            var settingKey = CreateQueryProductStatusSPSettingKey(
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

        private string CreateQueryProductStatusSPSettingKey(
            Level customerLevel,
            byte settingTypeId,
            byte settingTemplateId,
            byte productId)
        {
            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(settingTypeId, settingTemplateId, productId);

            return QueryProductStatusSpKeys[customerLevel.Value] + keySuffix;
        }

        public IResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            SettingType settingType,
            byte productId,
            Level uplineLevel)
        {
            var param = new Hashtable();
            param.Add("userid", settingContext.Actor.Id);
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("custid", customerId);
            param.Add("ip", settingContext.IP);
            param.Add("isdisabled", settingType.ToXml());
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            var updateSettingKey = "CustSetting_DisableStatUpd";
            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(
                ProductStatusConstants.SettingTypeId,
                ProductStatusConstants.SettingTempateId,
                productId);

            return ExecuteNonQuery(updateSettingKey + keySuffix, param);
        }
    }
}