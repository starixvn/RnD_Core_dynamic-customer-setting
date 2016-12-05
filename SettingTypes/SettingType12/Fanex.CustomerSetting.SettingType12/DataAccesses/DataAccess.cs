namespace Fanex.CustomerSetting.SettingType12.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using Constants;
    using CustomerSetting.DataAccesses.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Data;
    using Models;
    using Models.Requests;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        public IEnumerable<LossLimitItem> GetCustomerLossLimit(CustomerSettingQuery query)
        {
            using (var objectDb = new ObjectDb("CustSetting_LossLimitSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "productid", query.ProductId },
                    { "settingType", Constants.SettingTypeId }
                };

                return objectDb.Query<LossLimitItem>(parameterValues);
            }
        }

        public IResult UpdateCustomerLossLimit(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string lossLimitXml)
        {
            var param = new Hashtable
            {
                { "userid", settingContext.Actor.Id },
                { "subaccid", settingContext.Actor.SubAccountId },
                { "custid", customerId },
                { "settingtype", Constants.SettingTypeId },
                { "ip", settingContext.IP },
                { "limit", lossLimitXml },
                { "msgtype", 0 },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            return ExecuteNonQuery("CustSetting_LossLimitUpd", param);
        }

        public CurrentLossAmountItem GetCustomerCurrentLossAmount(CurrentLossAmountQuery query)
        {
            using (var objectDb = new ObjectDb("CustSetting_LossPercentSelect"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "sporttype", query.SportTypeId },
                    { "bettype", query.BetTypeId }
                };

                return objectDb.QueryEntity<CurrentLossAmountItem>(parameterValues);
            }
        }
    }
}