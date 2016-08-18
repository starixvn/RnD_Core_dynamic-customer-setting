namespace Fanex.CustomerSetting.NumberGames2.DataAccesses
{
    using Data;
    using Models;

    public class CustomerSyncStatusDataAccess : ICustomerSyncStatusDataAccess
    {
        public CustomerSyncStatus GetCustomserSyncStatus(int customerId, byte productId)
        {
            var customerSyncStatus = new CustomerSyncStatus
            {
                IsSynced = false,
                IsSyncedUpline = false
            };

            using (var objectDb = new ObjectDb("Acc_CustSetting_SyncGameStatusSelect"))
            {
                customerSyncStatus = objectDb.QueryEntity<CustomerSyncStatus>(new
                {
                    CustId = customerId,
                    ProductId = productId
                });
            }

            return customerSyncStatus;
        }

        public void UpdateCustomerSynceStatus(int customerId, byte productId, int userId, int subAccountId, string ip)
        {
            using (var objectDb = new ObjectDb("Acc_CustSetting_SyncGameStatusUpd"))
            {
                objectDb.ExecuteNonQuery(new
                {
                    CustID = customerId,
                    ProductID = productId,
                    UserID = userId,
                    SubAccID = subAccountId,
                    IP = ip
                });
            }
        }
    }
}