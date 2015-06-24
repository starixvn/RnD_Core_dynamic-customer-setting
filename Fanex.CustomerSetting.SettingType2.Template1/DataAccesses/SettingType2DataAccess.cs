namespace Fanex.CustomerSetting.SettingType2.Template1.DataAccesses
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
    using Fanex.CustomerSetting.SettingType2.Template1.Constants;
    using Fanex.CustomerSetting.SettingType2.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType2.Template1.Models;
    using Fanex.Data;

    public class SettingType2DataAccess : BaseDataAccess, ISettingTypeDataAccess
    {
        public SettingType GetCustomerSettingType(User actor, QuerySettingParams settingQuery)
        {
            var settingKey = GetSettingKeyForGettingBetLimit(settingQuery.CustomerLevel);
            IEnumerable<BetLimitItemDto> betLimitItemDtoList = null;
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingTypeSetting(settingQuery, SettingType2Constants.SettingType);
                betLimitItemDtoList = objectDb.Query<BetLimitItemDto>(parameterValues);
            }

            return new SettingType
            {
                SettingItems = ConvertToBetLimitItemList(betLimitItemDtoList)
            };
        }

        private IEnumerable<BetLimitItem> ConvertToBetLimitItemList(IEnumerable<BetLimitItemDto> betLimitItemDtoList)
        {
            return betLimitItemDtoList.Select<BetLimitItemDto, BetLimitItem>(
                betLimitDto => new BetLimitItem
                {
                    SportTypeId = betLimitDto.SportTypeId,
                    BetTypeId = betLimitDto.BetTypeId,
                    MaxBet = betLimitDto.MaxBet,
                    MinBet = betLimitDto.MinBet,
                    MaxPerMatch = betLimitDto.MaxBetMax,
                    Constraint = new BetLimitItem
                    {
                        MinBet = betLimitDto.MinBetMin,
                        MaxBet = betLimitDto.MaxBetMax,
                        MaxPerMatch = betLimitDto.MaxPerMatchMax
                    }
                });
        }

        private string GetSettingKeyForGettingBetLimit(CustomerLevel customerLevel)
        {
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    return "Acc_CustSetting_SuperBetLimitSelect";

                case CustomerLevel.Master:
                    return "Acc_CustSetting_MasterBetLimitSelect";

                case CustomerLevel.Agent:
                    return "Acc_CustSetting_AgentBetLimitSelect";

                case CustomerLevel.Member:
                    return "Acc_CustSetting_MemberBetLimitSelect";

                default:
                    throw new ArgumentOutOfRangeException("This customer level is not supported");
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
                BetLimit = settingType.ToXml()
            };

            return ExecuteNonQuery("Acc_CustSetting_BetLimitUpd", storedProcedureParams);
        }
    }
}