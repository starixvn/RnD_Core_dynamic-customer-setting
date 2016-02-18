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
            Check.Argument.IsNotNegativeOrZero(customerId, "customerId");
            Check.Argument.IsNotNegativeOrZero(productId, "productId");

            return dataAccess.GetCustomserProductStatus(customerId, productId);
        }

        public void UpdateCustomerStatus(int customerId, byte productId, int subAccountId, int userId, string ip)
        {
            Check.Argument.IsNotNegativeOrZero(customerId, "customerId");
            Check.Argument.IsNotNegativeOrZero(productId, "productId");
            Check.Argument.IsNotNegative(subAccountId, "subAccountId");
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotEmpty(ip, "ip");

            dataAccess.UpdateCustomerProductSyncedStatus(customerId, productId, subAccountId, userId, ip);
        }
    }
}