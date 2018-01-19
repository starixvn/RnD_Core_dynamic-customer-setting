namespace Fanex.CustomerSetting.Casino.Services
{
    public interface ICustomerProductSyncedStatusService
    {
        bool CheckCustomerProductSyncedStatus(int customerId, byte productId);

        void UpdateCustomerProductSyncedStatus(int customerId, byte productId, int userId, int subAccountId, string ip);
    }
}