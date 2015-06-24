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
    using Fanex.CustomerSetting.SettingType3.Template1.Constants;
    using Fanex.CustomerSetting.SettingType3.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType3.Template1.Models;
    using Fanex.Data;

    public class SettingType3DataAccess : BaseDataAccess, ISettingDataAccess
    {
        public SettingType GetCustomerSettingType(
            User actor,
            QuerySettingParams settingQuery)
        {
            var settingKey = GetSettingKeyForGettingCommission(settingQuery.CustomerLevel);
            IEnumerable<CommissionItemDto> commissionItemDtoList = null;
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingTypeSetting(settingQuery, SettingType3Constants.SettingType);
                commissionItemDtoList = objectDb.Query<CommissionItemDto>(parameterValues);
            }

            return new SettingType
            {
                SettingItems = ConvertToCommissionItems(commissionItemDtoList)
            };
        }

        private IEnumerable<CommissionItem> ConvertToCommissionItems(IEnumerable<CommissionItemDto> commissionItemDtoList)
        {
            return commissionItemDtoList.Select<CommissionItemDto, CommissionItem>(
                commItemDto => new CommissionItem
                {
                    SportTypeId = commItemDto.SportTypeId,
                    BetTypeId = commItemDto.BetTypeId,
                    CommRate = commItemDto.CommRate,
                    Upline = new CommissionItem
                    {
                        CommRate = commItemDto.UplineCommRate,
                        Upline = commItemDto.UplineOfUplineCommRate.HasValue ? new CommissionItem
                        {
                            CommRate = commItemDto.UplineOfUplineCommRate.Value
                        } 
                        : null
                    }
                });
        }

        private string GetSettingKeyForGettingCommission(CustomerLevel customerLevel)
        {
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    return "Acc_CustSetting_SuperCommissionSelect";

                case CustomerLevel.Master:
                    return "Acc_CustSetting_MasterCommissionSelect";

                case CustomerLevel.Agent:
                    return "Acc_CustSetting_AgentCommissionSelect";

                case CustomerLevel.Member:
                    return "Acc_CustSetting_MemberCommissionSelect";

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
                Discount = settingType.ToXml()
            };

            return ExecuteNonQuery("Acc_CustSetting_DiscountUpd", storedProcedureParams);
        }
    }
}