namespace Fanex.CustomerSetting.Casino.Tests.Services.Impl
{
    using Casino.Services;
    using Fanex.CustomerSetting.Casino.Services.Impl;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class CustomerProductSyncedStatusServiceTests
    {
        private ICustomerProductSyncedStatusService _customerProductSyncedStatusService;
        private IObjectDb subObjectDb;

        public CustomerProductSyncedStatusServiceTests()
        {
            _customerProductSyncedStatusService = new CustomerProductSyncedStatusService();
            subObjectDb = Substitute.For<IObjectDb>();

            ObjectDbFactory.ObjectDb = (settingKey) => subObjectDb;
        }

        [TestCase(-1, 1)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void CheckCustomerProductSyncedStatus_InvalidCustIdOrProductId_ThrowCorrespondingException(int custId, byte productId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _customerProductSyncedStatusService.CheckCustomerProductSyncedStatus(custId, productId));
        }

        [Test]
        public void CheckCustomerProductSyncedStatus_ValidCustIdOrProductId_RunSuccessfully()
        {
            Assert.DoesNotThrow(() => _customerProductSyncedStatusService.CheckCustomerProductSyncedStatus(1, 1));
        }

        [TestCase(0, 0, 0, 0, "127.0.0.0")]
        [TestCase(-1, 0, 0, 0, "127.0.0.0")]
        [TestCase(1, 0, 0, 0, "127.0.0.0")]
        [TestCase(1, 1, 0, 0, "127.0.0.0")]
        [TestCase(1, 1, -1, 0, "127.0.0.0")]
        [TestCase(1, 1, 1, -1, "127.0.0.0")]
        public void UpdateCustomerProductSyncedStatus_InvalidInputs_ThrowCorrespondingException(int customerId, byte productId, int userId, int subAccountId, string ip)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _customerProductSyncedStatusService.UpdateCustomerProductSyncedStatus(customerId, productId, userId, subAccountId, ip));
        }

        [Test]
        public void UpdateCustomerProductSyncedStatus_InvalidIP_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _customerProductSyncedStatusService.UpdateCustomerProductSyncedStatus(1, 1, 1, 0, string.Empty));
        }

        [Test]
        public void UpdateCustomerProductSyncedStatus_ValidInputs_RunSuccessfully()
        {
            Assert.DoesNotThrow(() => _customerProductSyncedStatusService.UpdateCustomerProductSyncedStatus(1, 1, 1, 1, "127.0.0.0"));
        }
    }
}
