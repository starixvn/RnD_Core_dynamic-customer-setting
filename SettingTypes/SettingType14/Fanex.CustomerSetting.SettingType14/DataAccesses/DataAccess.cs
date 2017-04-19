namespace Fanex.CustomerSetting.SettingType14.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using CustomerSetting.DataAccesses.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Data;
    using Dtos;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        public IEnumerable<TicketCommissionDto> GetCustomerTicketCommissionRawData(CustomerSettingQuery query)
        {
            using (var objectDb = new ObjectDb("Acc_CustSetting_TicketCommissionSelect"))
            {
                var parameters = new Hashtable
                {
                    { "custid", query.CustomerId },
                    { "productid", query.ProductId }
                };

                if (query.CustomerLevel.IsMemberLevel())
                {
                    parameters.Add("uplineroleid", query.UplineLevel.Value);
                }

                return objectDb.Query<TicketCommissionDto>(parameters);
            }
        }

        public IResult UpdateCustomerTicketCommission(
            ISettingContext settingContext,
            int customerId,
            string commissionXml)
        {
            var parameters = new Hashtable
            {
                { "userid", settingContext.Actor.Id },
                { "subaccid", settingContext.Actor.SubAccountId },
                { "custid", customerId },
                { "ip", settingContext.IP },
                { "commission", commissionXml },
                { "msgtype", 0 },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            return ExecuteNonQuery("Acc_CustSetting_TicketCommissionUpd", parameters);
        }
    }
}