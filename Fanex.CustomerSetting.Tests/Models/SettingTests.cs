namespace Fanex.CustomerSetting.Tests.Models
{
    using System;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using NUnit.Framework;

    public class SettingTests
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void SetCustomerId_ValueIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            Setting setting = InstanceFactory.CreateCustomerSetting();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () =>
                    {
                        setting.CustomerId = customerId;
                    }));
        }

        [Test]
        public void SetProductId_ValueIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            Setting setting = InstanceFactory.CreateCustomerSetting();

            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => setting.ProductId = 0));
        }
      
        [Test]
        public void SetSettingTypes_ValueIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            Setting setting = InstanceFactory.CreateCustomerSetting();

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                     () => setting.SettingTypes = null));
        }
    }
}