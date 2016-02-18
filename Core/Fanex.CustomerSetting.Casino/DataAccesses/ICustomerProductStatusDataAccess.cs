namespace Fanex.CustomerSetting.Casino.DataAccesses
{
    using Models;

    public interface ICustomerProductStatusDataAccess
    {
        CustomerProductStatus GetCustomserProductStatus(int customerId, byte productId);

        void UpdateCustomerProductSyncedStatus(int customerId, byte productId, int userId, int subAccountId, string ip);
    }
}