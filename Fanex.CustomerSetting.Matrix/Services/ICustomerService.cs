namespace Fanex.CustomerSetting.Matrix.Services
{
    using Fanex.CustomerSetting.Matrix.Models;

    public interface ICustomerService
    {
        Customer GetCustomer(string customerName);
    }
}