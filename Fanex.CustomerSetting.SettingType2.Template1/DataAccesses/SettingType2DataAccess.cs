namespace Fanex.CustomerSetting.SettingType2.Template1.DataAccesses
{
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
    using Fanex.CustomerSetting.SettingType2.Template1.Constants;
    using Fanex.CustomerSetting.SettingType2.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType2.Template1.Models;
    using Fanex.Data;

    public class SettingType2DataAccess : BaseDataAccess, ISettingTypeDataAccess
    {
        private const string SettingKeySuffix = "_2_1";

        public SettingType GetCustomerSettingType(User actor, QuerySettingParam queryParam)
        {
            var betLimitItemDtoList = ReadBetLimitItemsRawData(queryParam);

            return new SettingType
            {
                Name = Label.BetSetting,
                SettingItems = betLimitItemDtoList.Select<BetLimitItemDto, BetLimitItem>(betLimitDto => new BetLimitItem(betLimitDto))
            };
        }

        private IEnumerable<BetLimitItemDto> ReadBetLimitItemsRawData(QuerySettingParam queryParam)
        {
            var settingKey = GetSettingKeyForGettingSettingType(queryParam, BetLimitConstants.SettingTypeId, BetLimitConstants.SettingTempateId);
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameterValues = GetSPParamsForGettingSettingType(queryParam, BetLimitConstants.SettingTypeId);
                return objectDb.Query<BetLimitItemDto>(parameterValues);
            }
        }

        public IResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            Customer customer,
            SettingType settingType,
            byte productId, 
            CustomerLevel? uplineLevel)
        {
            var param = new Hashtable();
            param.Add("userid", settingContext.Actor.Id);
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("custid", customer.Id);
            param.Add("ip", settingContext.IP);
            param.Add("betlimit", settingType.ToXml());
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            var updateSettingKey = "CustSetting_BetLimitUpd_2_1";
            if (CustomerSettingSection.Settings.IsProductNotUseDefaultStoredProcedures(settingType.Id, settingType.SettingTemplate, productId))
            {
                updateSettingKey += string.Format("_{0}", productId);
            }

            return ExecuteNonQuery(updateSettingKey, param);
        }
    }
}