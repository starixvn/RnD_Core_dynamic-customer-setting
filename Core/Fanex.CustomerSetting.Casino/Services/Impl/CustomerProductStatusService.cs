namespace Fanex.CustomerSetting.Casino.Services.Impl
{
    using Common.Helper;
    using DataAccesses;
    using DataAccesses.Impl;
    using Models;

    public class CustomerProductStatusService : ICustomerProductStatusService
    {
        private readonly ICustomerProductStatusDataAccess dataAccess;

        public CustomerProductStatusService(ICustomerProductStatusDataAccess dataAccess = null)
        {
            this.dataAccess = dataAccess ?? new CustomerProductStatusDataAccess();
        }

        public CustomerProductStatus GetCustomerStatus(int customerId, byte productId)
        {
            Check.Argument.IsNotNegativeOrZero(customerId, nameof(customerId));
            Check.Argument.IsNotNegativeOrZero(productId, nameof(productId));

            return dataAccess.GetCustomserProductStatus(customerId, productId);
        }

        public void UpdateCustomerStatus(int customerId, byte productId, int userId, int subAccountId, string ip)
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