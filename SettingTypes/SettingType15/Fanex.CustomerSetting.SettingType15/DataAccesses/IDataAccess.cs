namespace Fanex.CustomerSetting.SettingType15.DataAccesses
{
    using System.Collections.Generic;
    using CustomerSetting.Models.Requests;
    using Dtos;

    internal interface IDataAccess
    {
        IEnumerable<WinningShareDto> GetCustomerWinningShare(CustomerSettingQuery query);
    }
}