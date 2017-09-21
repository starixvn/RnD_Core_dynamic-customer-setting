namespace Fanex.CustomerSetting.Casino.Services.Impl
{
    using Common.Helper;
    using DataAccesses;
    using DataAccesses.Impl;

    public class CustomerProductSyncedStatusService : ICustomerProductSyncedStatusService
    {
        private readonly ICustomerProducSyncedtStatusDataAccess dataAccess;

        public CustomerProductSyncedStatusService(ICustomerProducSyncedtStatusDataAccess dataAccess = null)
        {
            this.dataAccess = dataAccess ?? new CustomerProductSyncedStatusDataAccess();
        }

        public bool CheckCustomerProductSyncedStatus(int customerId, byte productId)
        {
            Check.Argument.IsNotNegativeOrZero(customerId, nameof(customerId));
            Check.Argument.IsNotNegativeOrZero(productId, nameof(productId));

            return dataAccess.CheckCustomerProductSyncedStatus(customerId, productId);
        }

        public void UpdateCustomerProductSyncedStatus(int customerId, byte productId, int userId, int subAccountId, string ip)
        {
            Check.Argument.IsNotNegativeOrZero(customerId, nameof(customerId));
            Check.Argument.IsNotNegativeOrZero(productId, nameof(productId));
            Check.Argument.IsNotNegativeOrZero(userId, nameof(userId));
            Check.Argument.IsNotNegative(subAccountId, nameof(subAccountId));
            Check.Argument.IsNotEmpty(ip, nameof(ip));

            dataAccess.UpdateCustomerProductSyncedStatus(customerId, productId, userId, subAccountId, ip);
        }
    }
}