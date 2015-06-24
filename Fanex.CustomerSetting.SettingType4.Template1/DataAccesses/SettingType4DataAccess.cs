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
    using Fanex.CustomerSetting.SettingType4.Template1.Constants;
    using Fanex.CustomerSetting.SettingType4.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType4.Template1.Models;
    using Fanex.Data;

    public class SettingType4DataAccess : BaseDataAccess, ISettingDataAccess
    {
        public SettingType GetCustomerSettingType(
            User actor,
            QuerySettingParams settingQuery)
        {
            var settingKey = GetSettingKeyForGettingProductStatus(settingQuery.CustomerLevel);
            IEnumerable<ProductStatusItemDto> productStatusItemDtoList = null;
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingTypeSetting(settingQuery, SettingType4Constants.SettingType);
                productStatusItemDtoList = objectDb.Query<ProductStatusItemDto>(parameterValues);
            }

            return new SettingType
            {
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

        private string GetSettingKeyForGettingProductStatus(CustomerLevel customerLevel)
        {
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    return "Acc_CustSetting_SuperProductStatusSelect";

                case CustomerLevel.Master:
                    return "Acc_CustSetting_MasterProductStatusSelect";

                case CustomerLevel.Agent:
                    return "Acc_CustSetting_AgentProductStatusSelect";

                case CustomerLevel.Member:
                    return "Acc_CustSetting_MemberProductStatusSelect";

                default:
                    throw new ArgumentOutOfRangeException("This customer level is not supported");
            }
        }

        public IResult UpdateCustomerSetting(
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

            return ExecuteNonQuery("Acc_CustSetting_DisableStatUpd", storedProcedureParams);
        }
    }
}