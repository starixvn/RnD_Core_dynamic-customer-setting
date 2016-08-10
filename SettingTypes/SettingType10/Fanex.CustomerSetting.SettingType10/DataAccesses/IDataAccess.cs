namespace Fanex.CustomerSetting.SettingType10.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Dtos;

    internal interface IDataAccess
    {
        IEnumerable<CommissionDto> GetSMAComissions(CustomerSettingQuery query);

        Tuple<IEnumerable<string>, string, IEnumerable<CommissionDto>> GetMemberComissions(CustomerSettingQuery query);

        IResult UpdateCustomerCommission(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string commissionXml);

        IEnumerable<CommissionDto> GetMemberComissionGroupConstraints(int memberId, string userGroup);
    }
}