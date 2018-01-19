namespace Fanex.CustomerSetting.Casino.DataAccesses
{
    public interface ICustomerProducSyncedtStatusDataAccess
    {
        bool CheckCustomerProductSyncedStatus(int customerId, byte productId);

        void UpdateCustomerProductSyncedStatus(int customerId, byte productId, int userId, int subAccountId, string ip);
    }
}