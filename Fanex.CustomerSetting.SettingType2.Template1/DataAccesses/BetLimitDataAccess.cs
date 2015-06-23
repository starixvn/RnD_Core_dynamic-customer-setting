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

    public class BetLimitDataAccess : BaseDataAccess, ISettingDataAccess
    {
        public SettingType GetCustomerSettingType(User actor, SettingQuery settingQuery)
        {
            var settingKey = GetSettingKeyForGettingBetLimit(settingQuery.CustomerLevel);
            IEnumerable<BetLimitItemDto> betLimitItemDtoList = null;
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                betLimitItemDtoList = objectDb.Query<BetLimitItemDto>(GetSPParamsForGettingBetLimit(settingQuery));
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
                    return "Acc_CustSetting_SuperSettingSelect";

                case CustomerLevel.Master:
                    return "Acc_CustSetting_MasterSettingSelect";

                case CustomerLevel.Agent:
                    return "Acc_CustSetting_AgentSettingSelect";

                case CustomerLevel.Member:
                    return "Acc_CustSetting_MemberSettingSelect";

                default:
                    throw new ArgumentOutOfRangeException("This customer level is not supported");
            }
        }

        private dynamic GetSPParamsForGettingBetLimit(SettingQuery settingQuery)
        {
            switch (settingQuery.CustomerLevel)
            {
                case CustomerLevel.Super:
                case CustomerLevel.Master:
                case CustomerLevel.Agent:
                    return new
                    {
                        CustId = settingQuery.CustomerId,
                        ProductId = settingQuery.ProductId,
                        SettingType = SettingType2Constants.SettingType
                    };

                case CustomerLevel.Member:
                    return new
                    {
                        CustId = settingQuery.CustomerId,
                        ProductId = settingQuery.ProductId,
                        RoleId = settingQuery.UplineLevel.Value,
                        SettingType = SettingType2Constants.SettingType
                    };

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
                SubAccId = settingContext.SubAccount.Id,
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