namespace Fanex.CustomerSetting.Matrix.Services
{
    using System.Collections.Generic;
    using Models;

    public interface ICustomerService
    {
        IEnumerable<Customer> GetCustomerLine(string customerName);
    }
}