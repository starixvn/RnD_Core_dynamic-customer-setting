namespace Fanex.CustomerSetting.SettingType3.Template1.DataAccesses
{
    using System;
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
    using Fanex.CustomerSetting.SettingType3.Template1.Constants;
    using Fanex.CustomerSetting.SettingType3.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType3.Template1.Models;
    using Fanex.Data;

    public class SettingType3DataAccess : BaseDataAccess, ISettingTypeDataAccess
    {
        private const string SettingKeySuffix = "_3_1";

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
            var settingKey = GetSettingKeyForGettingSettingType(queryParam, SettingType3Constants.SettingTypeId, SettingType3Constants.SettingTempateId);
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(queryParam, SettingType3Constants.SettingTypeId);
                return objectDb.Query<CommissionItemDto>(parameterValues);
            }
        }

        public IResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            Customer customer,
            SettingType settingType,
            byte productId, 
            CustomerLevel? uplineLevel)
        {
            var storedProcedureParams = new
            {
                UserId = settingContext.Actor.Id,
                SubAccId = settingContext.Actor.SubAccountId,
                CustId = customer.Id,
                Ip = settingContext.IP,
                ErrCode = 0,
                ErrMsg = string.Empty,
                MsgType = 0,
                Discount = settingType.ToXml()
            };

            var updateSettingKey = "CustSetting_DiscountUpd_3_1";
            if (CustomerSettingSection.Settings.IsProductDoesNotUseDefaultStoredProcedures(settingType.Id, settingType.SettingTemplate, productId))
            {
                updateSettingKey += string.Format("_{0}", productId);
            }

            return ExecuteNonQuery(updateSettingKey, storedProcedureParams);
        }
    }
}