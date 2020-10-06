namespace Fanex.CustomerSetting.NumberGames2.Tests.Services.Impl
{
    using Fanex.CustomerSetting.NumberGames2.DataAccesses;
    using Fanex.CustomerSetting.NumberGames2.Models;
    using Fanex.CustomerSetting.NumberGames2.Services.Impl;
    using Fanex.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections;

    [TestFixture]
    class CustomerSyncStatusServiceTests
    {
        private CustomerSyncStatusService customerSyncStatusService;
        private ICustomerSyncStatusDataAccess subCustomerSyncStatusDataAccess;
        private IObjectDb subObjectDb;

        [SetUp]
        public void SetUp()
        {
            subCustomerSyncStatusDataAccess = Substitute.For<ICustomerSyncStatusDataAccess>();
            subCustomerSyncStatusDataAccess.GetCustomserSyncStatus(Arg.Any<int>(), Arg.Any<byte>()).Returns(new CustomerSyncStatus());

            subObjectDb = Substitute.For<IObjectDb>();
            subObjectDb.ExecuteNonQuery(Arg.Any<Hashtable>()).Returns(0);
            subObjectDb.QueryEntity<CustomerSyncStatus>(Arg.Any<Hashtable>()).Returns(new CustomerSyncStatus());
            customerSyncStatusService = new CustomerSyncStatusService(subCustomerSyncStatusDataAccess);
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(-1, 1)]
        [TestCase(-1, 0)]
        public void GetCustomerSyncStatus_InvalidCustIdAndProductId_ThrowArgumentOutOfRangeException(int customerId, byte productId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => customerSyncStatusService.GetCustomerSyncStatus(customerId, productId));
        }

        [Test]
        [TestCase(1, 1)]
        public void GetCustomerSyncStatus_ValidCustIdAndProductId_DoesNotThrowException(int customerId, byte productId)
        {
            Assert.DoesNotThrow(
                () => customerSyncStatusService.GetCustomerSyncStatus(customerId, productId));
        }

        [Test]
        [TestCase(0, 0, 0, 0, "")]
        [TestCase(0, 0, 0, 0, "test")]
        [TestCase(0, 0, 0, -1, "")]
        [TestCase(0, 0, 0, -1, "test")]
        [TestCase(0, 0, 1, 0, "")]
        [TestCase(0, 0, 1, 0, "test")]
        [TestCase(0, 0, 1, -1, "")]
        [TestCase(0, 0, 1, -1, "test")]
        [TestCase(0, 1, 0, 0, "")]
        [TestCase(0, 1, 0, 0, "test")]
        [TestCase(0, 1, 0, -1, "")]
        [TestCase(0, 1, 0, -1, "test")]
        [TestCase(0, 1, 1, 0, "")]
        [TestCase(0, 1, 1, 0, "test")]
        [TestCase(0, 1, 1, -1, "")]
        [TestCase(0, 1, 1, -1, "test")]
        [TestCase(1, 0, 0, 0, "")]
        [TestCase(1, 0, 0, 0, "test")]
        [TestCase(1, 0, 0, -1, "")]
        [TestCase(1, 0, 0, -1, "test")]
        [TestCase(1, 0, 1, 0, "")]
        [TestCase(1, 0, 1, 0, "test")]
        [TestCase(1, 0, 1, -1, "")]
        [TestCase(1, 0, 1, -1, "test")]
        [TestCase(1, 1, 0, 0, "")]
        [TestCase(1, 1, 0, 0, "test")]
        [TestCase(1, 1, 0, -1, "")]
        [TestCase(1, 1, 0, -1, "test")]
        [TestCase(1, 1, 1, 0, "")]
        [TestCase(1, 1, 1, -1, "")]
        [TestCase(1, 1, 1, -1, "test")]
        public void UpdateCustomerSyncStatus_InvalidInputs_ThrowArgumentOutOfRangeException(int customerId, byte productId, int userId, int subAccountId, string ip)
        {
            Assert.Catch(() => customerSyncStatusService.UpdateCustomerSyncStatus(customerId, productId, userId, subAccountId, ip));
        }

        [Test]
        [TestCase(1, 1, 1, 0, "test")]
        public void UpdateCustomerSyncStatus_ValidInputs_DoesNotThrowException(int customerId, byte productId, int userId, int subAccountId, string ip)
        {
            Assert.DoesNotThrow(() => customerSyncStatusService.UpdateCustomerSyncStatus(customerId, productId, userId, subAccountId, ip));
        }
    }
}
