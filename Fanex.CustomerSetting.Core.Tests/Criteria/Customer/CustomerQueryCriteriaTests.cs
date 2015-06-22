namespace Fanex.CustomerSetting.Core.Tests.SPObjects.Criteria.Customer
{
    using Fanex.CustomerSetting.Core.Criteria.Customer;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerQueryCriteriaTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        public void IsValid_CustomerIdIsNegativeOrZero_ReturnFalse(int customerId)
        {
            //// Arrange
            var criteria = CreateCustomerQueryCriteriaFakeObject();
            criteria.CustId = customerId;

            //// Act
            var actualResult = criteria.IsValid();

            //// Arrange
            Assert.IsFalse(actualResult);
        }

        [TestCase(1)]
        [TestCase(1000)]
        public void IsValid_CustomerIdIsGreaterThanZero_ReturnTrue(int customerId)
        {
            //// Arrange
            var criteria = CreateCustomerQueryCriteriaFakeObject();
            criteria.CustId = customerId;

            //// Act
            var actualResult = criteria.IsValid();

            //// Arrange
            Assert.IsTrue(actualResult);
        }

        private CustomerQueryCriteria CreateCustomerQueryCriteriaFakeObject()
        {
            return new CustomerQueryCriteria
            {
                CustId = 1
            };
        }
    }
}