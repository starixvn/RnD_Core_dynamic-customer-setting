namespace Fanex.CustomerSetting.Tests.Criteria
{
    using Fanex.CustomerSetting.Criteria;
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

        [Test]
        public void GetSettingKey_ReturnSettingKey()
        {
            //// Arrange
            var criteria = CreateCustomerQueryCriteriaFakeObject();

            //// Act
            var actualSettingKey = criteria.GetSettingKey();

            //// Arrange
            Assert.AreEqual("Acc_CustSetting_UserInfoSelect", actualSettingKey);
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