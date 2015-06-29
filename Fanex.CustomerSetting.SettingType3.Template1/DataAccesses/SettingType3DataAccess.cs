namespace Fanex.CustomerSetting.SettingType3.Template1.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Resources;
    using Fanex.CustomerSetting.SettingType3.Template1.Constants;
    using Fanex.CustomerSetting.SettingType3.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType3.Template1.Models;
    using Fanex.Data;

    public class SettingType3DataAccess : BaseDataAccess, ISettingTypeDataAccess
    {
        private const string SettingKeySuffix = "_3_1";

        public SettingType GetCustomerSettingType(
            User actor,
            QuerySettingParam settingQuery)
        {
            var commissionItemDtoList = ReadCommissionItemsRawData(settingQuery);

            return new SettingType
            {
                Name = Lang.Commission,
                SettingItems = commissionItemDtoList.Select<CommissionItemDto, CommissionItem>(commItemDto => new CommissionItem(commItemDto))
            };
        }

        private IEnumerable<CommissionItemDto> ReadCommissionItemsRawData(QuerySettingParam settingQuery)
        {
            var settingKey = GetSettingKeyForGettingSettingType(settingQuery.CustomerLevel, SettingKeySuffix);
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(settingQuery, SettingType3Constants.SettingType);
                return objectDb.Query<CommissionItemDto>(parameterValues);
            }
        }

        public IResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            Customer customer,
            SettingType settingType,
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

            return ExecuteNonQuery("Acc_CustSetting_DiscountUpd_3_1", storedProcedureParams);
        }
    }
}