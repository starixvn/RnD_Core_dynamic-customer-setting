namespace Fanex.CustomerSetting.Casino.Services
{
    using Models;

    public interface ICustomerProductStatusService
    {
        CustomerProductStatus GetCustomerStatus(int customerId, byte productId);

        void UpdateCustomerStatus(int customerId, byte productId, int subAccountId, int userId, string ip);
    }
}