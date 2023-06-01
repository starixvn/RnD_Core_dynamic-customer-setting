namespace Fanex.CustomerSetting.SettingType22.DataAccesses
{
    using CustomerSetting.DataAccesses.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Data;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Extensions;
    using Models;
    using Models.Requests;
    using System.Collections;
    using System.Collections.Generic;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        public IEnumerable<WinLossLimitItem> GetCustomerWinLossLimit(CustomerSettingQuery query)
        {
            using (var objectDb = ObjectDbFactory.ObjectDb("CustSetting_WinLossLimitSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "productid", query.ProductId },
                    { "settingtype", Constants.Constants.SettingTypeId },
                    { "settingstatus", query.SettingStatus }
                };

                return objectDb.Query<WinLossLimitItem>(parameterValues);
            }
        }

        public IResult UpdateCustomerWinLossLimit(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string winLossLimitXml)
        {
            var param = new Hashtable
            {
                { "userid", settingContext.Actor.Id },
                { "subaccid", settingContext.Actor.SubAccountId },
                { "custid", customerId },
                { "ip", settingContext.IP },
                { "limit", winLossLimitXml },
                { "settingtype", Constants.Constants.SettingTypeId },
                { "settingstatus", AppSettings.SettingStatus },
                { "msgtype", 0 },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            return ExecuteNonQuery("CustSetting_WinLossLimitUpd", param);
        }

        public IEnumerable<CurrentWinLossAmountItem> GetCustomerCurrentWinLossAmount(CurrentWinLossAmountQuery query)
        {
            var siteDB = ObjectDbExtensions.GetSiteDBConfig(query.CustomerId);
            using (var objectDb = new ObjectDb($"{siteDB.PrefixSPName}CustSetting_WinLossPercentSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "sporttype", query.SportTypeId },
                    { "bettype", query.BetTypeId }
                };

                return objectDb.QueryOnSiteDB<CurrentWinLossAmountItem>(parameterValues, siteDB.AGIP);
            }
        }
    }
}