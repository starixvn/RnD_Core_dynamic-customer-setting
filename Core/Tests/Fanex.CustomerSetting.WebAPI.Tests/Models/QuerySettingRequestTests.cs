namespace Fanex.CustomerSetting.WebAPI.Tests.Models
{
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using NUnit.Framework;
    using WebApi.Models.Request;

    [TestFixture]
    public class QuerySettingRequestTests
    {
        private QuerySettingRequest _request;

        [SetUp]
        public void SetUp()
        {
            _request = new QuerySettingRequest
            {
                ProductId = 1,
                CustomerId = 1,
                CustomerLevel = Level.Member.Value,
                UplineLevel = Level.Agent.Value
            };
        }

        [Test]
        public void GetCustomerSettingQuery_Always_ReturnObject()
        {
            // Act.
            var query = _request.GetCustomerSettingQuery();

            // Assert.
            Assert.NotNull(query);
        }

        [Test]
        public void GetCustomerSettingQuery_Always_ReturnTypeOfCustomerSettingQuery()
        {
            // Act.
            var query = _request.GetCustomerSettingQuery();

            // Assert.
            Assert.IsAssignableFrom<CustomerSettingQuery>(query);
        }

        [Test]
        public void GetCustomerSettingQuery_Always_ReturnedObjectContainsExpectedData()
        {
            // Act.
            var query = _request.GetCustomerSettingQuery();

            // Assert.
            Assert.AreEqual(1, query.ProductId);
            Assert.AreEqual(1, query.CustomerId);
            Assert.AreEqual(Level.Member, query.CustomerLevel);
            Assert.AreEqual(Level.Agent, query.UplineLevel);
        }
    }
}