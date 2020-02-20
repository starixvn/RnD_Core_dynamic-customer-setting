namespace Fanex.CustomerSetting.Tests.Criteria
{
    using CustomerSetting.Criteria;
    using CustomerSetting.Models.Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerQueryCriteriaTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        public void IsValid_CustomerIdIsNegativeOrZero_ReturnFalse(int customerId)
        {
            //// Arrange
            var criteria = CreateCustomerQueryCriteriaInstance();
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
            var criteria = CreateCustomerQueryCriteriaInstance();
            criteria.CustId = customerId;

            //// Act
            var actualResult = criteria.IsValid();

            //// Arrange
            Assert.IsTrue(actualResult);
        }

        [TestCase(SystemType.Nap)]
        [TestCase(SystemType.InternalAlpha)]
        [TestCase(SystemType.Y7InternalAlpha)]
        [TestCase(SystemType.Alpha)]
        [TestCase(SystemType.Y7Alpha)]
        public void GetSettingKey_SystemIsNotExtNap_ReturnSettingKeyOfAccSystem(byte system)
        {
            //// Arrange
            var criteria = CreateCustomerQueryCriteriaInstance();
            criteria.System = system;

            //// Act
            var actualSettingKey = criteria.GetSettingKey();

            //// Arrange
            Assert.AreEqual("CustSetting_CustomerInfoSelect", actualSettingKey);
        }

        [TestCase(SystemType.ExternalNap)]
        [TestCase(SystemType.Y7ExternalNap)]
        [TestCase(SystemType.ExternalNapBetSetting)]
        [TestCase(SystemType.Y7ExternalNapBetSetting)]
        public void GetSettingKey_SystemIsExtNap_ReturnSettingKeyOfExtNapSystem(byte system)
        {
            //// Arrange
            var criteria = CreateCustomerQueryCriteriaInstance();
            criteria.System = system;

            //// Act
            var actualSettingKey = criteria.GetSettingKey();

            //// Arrange
            Assert.AreEqual("EAS_CustSetting_CustomerInfoSelect", actualSettingKey);
        }

        private CustomerInfoQueryCriteria CreateCustomerQueryCriteriaInstance()
        {
            return new CustomerInfoQueryCriteria
            {
                CustId = 1
            };
        }
    }
}