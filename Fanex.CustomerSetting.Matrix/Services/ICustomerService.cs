namespace Fanex.CustomerSetting.Matrix.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Matrix.Models;

    public interface ICustomerService
    {
        IEnumerable<Customer> GetCustomerLine(string customerName);
    }
}