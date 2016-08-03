namespace Fanex.CustomerSetting.NumberGames2.Services
{
    using Models;

    public interface ICustomerSyncStatusService
    {
        CustomerSyncStatus GetCustomerSyncStatus(int customerId, byte productId);

        void UpdateCustomerSyncStatus(int customerId, byte productId, int userId, int subAccountId, string ip);
    }
}