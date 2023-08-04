namespace Fanex.CustomerSetting.SettingType6.DataAccesses
{
    using CustomerSetting.DataAccesses.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Extensions;
    using Fanex.Data;
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
                { "settingstatus", AppSettings.SettingStatus },
                { "msgtype", 0 },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            return ExecuteNonQuery("CustSetting_WinLossLimitUpd", param);
        }

        public CurrentWinLossAmountItem GetCustomerCurrentWinLossAmount(CurrentWinLossAmountQuery query)
        {
            var siteDB = ObjectDbExtensions.GetSiteDBConfig(query.CustomerId);
            using (var objectDb = new ObjectDb($"{siteDB.PrefixSPName}CustSetting_WinLossPercentSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "sporttype", query.SportTypeId },
                    { "bettype", query.BetTypeId },
                    { "settingstatus", query.SettingStatus }
                };

                return objectDb.QueryEntityOnSiteDB<CurrentWinLossAmountItem>(parameterValues, siteDB.AGIP);
            }
        }
    }
}