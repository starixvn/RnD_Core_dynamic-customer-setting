namespace Fanex.CustomerSetting.NumberGames2.DataAccesses
{
    using Models;

    public interface ICustomerSyncStatusDataAccess
    {
        CustomerSyncStatus GetCustomserSyncStatus(int customerId, byte productId);

        void UpdateCustomerSynceStatus(int customerId, byte productId, int userId, int subAccountId, string ip);
    }
}