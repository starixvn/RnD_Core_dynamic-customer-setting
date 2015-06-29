namespace Fanex.CustomerSetting.SettingType4.Template1.DataAccesses
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
    using Fanex.CustomerSetting.SettingType4.Template1.Constants;
    using Fanex.CustomerSetting.SettingType4.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType4.Template1.Models;
    using Fanex.Data;

    public class SettingType4DataAccess : BaseDataAccess, ISettingTypeDataAccess
    {
        private const string SettingKeySuffix = "_4_1";

        public SettingType GetCustomerSettingType(
            User actor,
            QuerySettingParams settingQuery)
        {
            var settingKey = GetSettingKeyForGettingSettingType(settingQuery.CustomerLevel, SettingKeySuffix);
            IEnumerable<ProductStatusItemDto> productStatusItemDtoList = null;
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(settingQuery, SettingType4Constants.SettingType);
                productStatusItemDtoList = objectDb.Query<ProductStatusItemDto>(parameterValues);
            }

            return new SettingType
            {
                Name = Lang.ProductStatus,
                SettingItems = ConvertToProductStatusItems(productStatusItemDtoList)
            };
        }

        private IEnumerable<ProductStatusItem> ConvertToProductStatusItems(IEnumerable<ProductStatusItemDto> productStatusItemDtoList)
        {
            return productStatusItemDtoList.Select<ProductStatusItemDto, ProductStatusItem>(
                productStatusItemDto => new ProductStatusItem
                {
                    SportTypeId = productStatusItemDto.SportTypeId,
                    BetTypeId = productStatusItemDto.BetTypeId,
                    IsDisabled = productStatusItemDto.IsDisabled,
                    Downline = productStatusItemDto.IsDownlineDisabled.HasValue ? new ProductStatusItem
                    {
                        IsDisabled = productStatusItemDto.IsDownlineDisabled.Value
                    }
                    : null,
                    Upline = productStatusItemDto.IsUplineDisabled.HasValue ? new ProductStatusItem
                    {
                        IsDisabled = productStatusItemDto.IsUplineDisabled.Value
                    }
                    : null
                });
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
                IsDisabled = settingType.ToXml()
            };

            return ExecuteNonQuery("Acc_CustSetting_DisableStatUpd_4_1", storedProcedureParams);
        }
    }
}