namespace Fanex.CustomerSetting.Casino.Services
{
    using Models;

    public interface ICustomerProductStatusService
    {
        CustomerProductStatus GetCustomerStatus(int customerId, byte productId);

        void UpdateCustomerStatus(int customerId, byte productId, int userId, int subAccountId, string ip);
    }
}