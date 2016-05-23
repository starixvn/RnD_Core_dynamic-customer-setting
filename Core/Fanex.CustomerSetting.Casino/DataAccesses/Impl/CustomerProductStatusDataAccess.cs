namespace Fanex.CustomerSetting.Casino.DataAccesses.Impl
{
    using Data;
    using Models;

    public class CustomerProductStatusDataAccess : ICustomerProductStatusDataAccess
    {
        public CustomerProductStatus GetCustomserProductStatus(int customerId, byte productId)
        {
            var customerProductStatus = new CustomerProductStatus { IsSynced = false, IsDownlineSynced = false, IsHidden = true };
            using (var objectDb = new ObjectDb("Acc_CustSetting_SyncStatusSelect"))
            {
                customerProductStatus = objectDb.QueryEntity<CustomerProductStatus>(new { CustId = customerId, ProductId = productId });
            }

            return customerProductStatus;
        }

        public void UpdateCustomerProductSyncedStatus(int customerId, byte productId, int userId, int subAccountId, string ip)
        {
            using (var objectDb = new ObjectDb("Acc_CustSetting_SyncStatusUpd"))
            {
                objectDb.ExecuteNonQuery(new
                {
                    CustID = customerId,
                    ProductID = productId,
                    SubAccID = subAccountId,
                    UserID = userId,
                    IP = ip
                });
            }
        }
    }
}