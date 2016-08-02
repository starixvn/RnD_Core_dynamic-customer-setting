namespace Fanex.CustomerSetting.NumberGames2.Services.Impl
{
    using Common.Helper;
    using DataAccesses;
    using DataAccesses.Impl;
    using Models;

    public class CustomerSyncStatusService : ICustomerSyncStatusService
    {
        private readonly ICustomerSyncStatusDataAccess _dataAccess;

        public CustomerSyncStatusService(ICustomerSyncStatusDataAccess dataAccess = null)
        {
            _dataAccess = dataAccess ?? new CustomerSyncStatusDataAccess();
        }

        public CustomerSyncStatus GetCustomerSyncStatus(int customerId, byte productId)
        {
            Check.Argument.IsNotNegativeOrZero(customerId, nameof(customerId));
            Check.Argument.IsNotNegativeOrZero(productId, nameof(productId));

            return _dataAccess.GetCustomserSyncStatus(customerId, productId);
        }

        public void UpdateCustomerSyncStatus(int customerId, byte productId, int subAccountId, int userId, string ip)
        {
            Check.Argument.IsNotNegativeOrZero(customerId, nameof(customerId));
            Check.Argument.IsNotNegativeOrZero(productId, nameof(productId));
            Check.Argument.IsNotNegative(subAccountId, nameof(subAccountId));
            Check.Argument.IsNotNegativeOrZero(userId, nameof(userId));
            Check.Argument.IsNotEmpty(ip, nameof(ip));

            _dataAccess.UpdateCustomerSynceStatus(customerId, productId, userId, subAccountId, ip);
        }
    }
}