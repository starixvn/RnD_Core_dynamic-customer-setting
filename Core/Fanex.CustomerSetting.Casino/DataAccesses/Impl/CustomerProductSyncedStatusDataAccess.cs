namespace Fanex.CustomerSetting.Casino.DataAccesses.Impl
{
    using Fanex.CustomerSetting.DataAccesses;
    using System;

    public class CustomerProductSyncedStatusDataAccess : ICustomerProducSyncedtStatusDataAccess
    {
        public bool CheckCustomerProductSyncedStatus(int customerId, byte productId)
        {
            using (var objectDb = ObjectDbFactory.ObjectDb("Acc_CustSetting_DisableNewProductSyncStatusSelect"))
            {
                object syncedStatus = objectDb.ExecuteScalar(new { CustId = customerId, ProductId = productId });

                return Convert.ToBoolean(syncedStatus);
            }
        }

        public void UpdateCustomerProductSyncedStatus(int customerId, byte productId, int userId, int subAccountId, string ip)
        {
            using (var objectDb = ObjectDbFactory.ObjectDb("Acc_CustSetting_DisableNewProductSyncStatusUpd"))
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