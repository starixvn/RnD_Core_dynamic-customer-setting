namespace Fanex.CustomerSetting.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class SettingTests
    {
        [Test]
        public void SetProductId_ValueIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            Setting setting = InstanceFactory.CreateCustomerSetting();

            //// Act
            Action action = () => { setting.ProductId = 0; };

            //// Assert
            action.ShouldThrow<ArgumentOutOfRangeException>("Product Id cannot be negative or zero.");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CustomerId_SetValueIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            Setting setting = InstanceFactory.CreateCustomerSetting();

            //// Act
            Action action = () => { setting.CustomerId = customerId; };

            //// Assert
            action.ShouldThrow<ArgumentOutOfRangeException>("Product Id cannot be negative or zero.");
        }

        [Test]
        public void CustomerId_SetValidValueThenGet_ReturnSetValue()
        {
            //// Arrange
            Setting setting = InstanceFactory.CreateCustomerSetting();
            var customerId = 100;

            //// Act
            setting.CustomerId = customerId;

            //// Assert
            setting.CustomerId.ShouldBeEquivalentTo<int>(customerId);
        }

        [Test]
        public void CustomerLevel_SetNullValue_ThrowArgumentNullException()
        {
            //// Arrange
            Setting setting = InstanceFactory.CreateCustomerSetting();

            //// Act
            Action action = () => { setting.CustomerLevel = null; };

            //// Assert
            action.ShouldThrow<ArgumentNullException>("value");
        }

        [TestCase(0)]
        [TestCase(5)]
        [TestCase(6)]
        public void CustomerLevel_SetValueLessThanMemberLevelOrGreaterThanSuper_ThrowArgumentOutOfRangeException(byte levelValue)
        {
            //// Arrange
            Setting setting = InstanceFactory.CreateCustomerSetting();

            //// Act
            Action action = () => { setting.CustomerLevel = Enumeration.FromValue<Level>(levelValue); };

            //// Assert
            action.ShouldThrow<ArgumentOutOfRangeException>("Invalid customer level.");
        }

        [Test]
        public void SettingTypes_SetValueToNullThenGet_ReturnEmptySettingTypes()
        {
            //// Arrange
            var setting = new Setting(1, Level.Super, 1, null);

            //// Assert
            setting.SettingTypes.ShouldBeEquivalentTo<IEnumerable<SettingType>>(Enumerable.Empty<SettingType>());
        }
    }
}