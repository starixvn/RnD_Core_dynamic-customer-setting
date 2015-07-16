namespace Fanex.CustomerSetting.SettingType3.Template1.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.DataAccesses;
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

    public class SettingType3DataAccess : BaseDataAccess, ISettingTypeDataAccess
    {
        public SettingType GetCustomerSettingType(
            User actor,
            QuerySettingParam queryParam)
        {
            var commissionItemDtoList = ReadCommissionItemsRawData(queryParam);

            return new SettingType
            {
                Name = Label.Commission,
                SettingItems = commissionItemDtoList.Select<CommissionItemDto, CommissionItem>(commItemDto => new CommissionItem(commItemDto))
            };
        }

        private IEnumerable<CommissionItemDto> ReadCommissionItemsRawData(QuerySettingParam queryParam)
        {
            var settingKey = GetSettingKeyForQuerying(
                queryParam.CustomerLevel,
                CommissionConstants.SettingTypeId,
                CommissionConstants.SettingTempateId,
                queryParam.ProductId,
                queryParam.UplineLevel);

            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(queryParam, CommissionConstants.SettingTypeId);
                return objectDb.Query<CommissionItemDto>(parameterValues);
            }
        }

        private string GetSettingKeyForQuerying(
            UserLevel customerLevel, 
            byte settingTypeId, 
            byte settingTemplateId, 
            byte productId,
            UserLevel uplineLevel)
        {
            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(settingTypeId, settingTemplateId, productId);

            switch (customerLevel.Value)
            {
                case Level.Super:
                    return "CustSetting_SuperCommissionSelect" + keySuffix;

                case Level.Master:
                    return "CustSetting_MasterCommissionSelect" + keySuffix;

                case Level.Agent:
                    return "CustSetting_AgentCommissionSelect" + keySuffix;

                case Level.Member:
                    return "CustSetting_MemberCommissionSelect" + keySuffix;

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
            param.Add("discount", settingType.ToXml());
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            var updateSettingKey = "CustSetting_DiscountUpd";
            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(
                CommissionConstants.SettingTypeId,
                CommissionConstants.SettingTempateId,
                productId);

            return ExecuteNonQuery(updateSettingKey + keySuffix, param);
        }
    }
}