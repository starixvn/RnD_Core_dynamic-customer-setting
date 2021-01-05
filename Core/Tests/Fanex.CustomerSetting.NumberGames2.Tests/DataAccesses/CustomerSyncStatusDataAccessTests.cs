namespace Fanex.CustomerSetting.NumberGames2.Tests.DataAccesses
{
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.NumberGames2.DataAccesses;
    using Fanex.CustomerSetting.NumberGames2.Models;
    using Fanex.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System.Web.Script.Serialization;

    [TestFixture]
    public class CustomerSyncStatusDataAccessTests
    {
        ICustomerSyncStatusDataAccess customerSyncStatusDataAccess;
        IObjectDb subObjectDb;
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        public CustomerSyncStatusDataAccessTests()
        {
            customerSyncStatusDataAccess = new CustomerSyncStatusDataAccess();
            subObjectDb = Substitute.For<IObjectDb>();
            subObjectDb.QueryEntity<CustomerSyncStatus>(Arg.Any<object>()).Returns(new CustomerSyncStatus
            {
                IsSynced = false,
                IsSyncedUpline = false
            });

            ObjectDbFactory.ObjectDb = (settingKey) => subObjectDb;
        }

        [Test]
        public void GetCustomserSyncStatus_Always_ReturnCustomerSyncStatus()
        {
            var expected = new CustomerSyncStatus
            {
                IsSynced = false,
                IsSyncedUpline = false
            };

            var actual = customerSyncStatusDataAccess.GetCustomserSyncStatus(Arg.Any<int>(), Arg.Any<byte>());
            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual));
        }

        [Test]
        public void UpdateCustomerSynceStatus_Always_NoException()
        {
            Assert.DoesNotThrow(() => customerSyncStatusDataAccess.UpdateCustomerSynceStatus(1, 1, 1, 1, string.Empty));
        }
    }
}
