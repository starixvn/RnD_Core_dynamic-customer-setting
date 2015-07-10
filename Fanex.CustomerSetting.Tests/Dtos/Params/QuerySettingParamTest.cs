namespace Fanex.CustomerSetting.Tests.Dtos.Params
{
    using System;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class QuerySettingParamTest
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void SetCustomerId_ValueIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam(0, CustomerLevel.Master, 0, null)));
        }

        [Test]
        public void SetProductId_ValueIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam(1, CustomerLevel.Master, 0, null)));
        }

        [Test]
        public void SetCustomerLevel_ValueIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam(1, (CustomerLevel)0, 1, null)));
        }

        [TestCase(5)]
        [TestCase(10)]
        public void SetCustomerLevel_ValueIsLargerThanSuperValue_ThrowArgumentOutOfRangeException(byte customerLevelValue)
        {
            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                     () => new QuerySettingParam(1, (CustomerLevel)customerLevelValue, 1, null)));
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void SetUplineLevel_ValueIsNotNullWhenCustomerIsNotMember_ThrowArgumentOutOfRangeException(CustomerLevel customerLevel)
        {
            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam(1, customerLevel, 1, customerLevel)));
        }

        [TestCase(5)]
        [TestCase(10)]
        public void SetUplineLevel_ValueIsLargerThanSuperValue_ThrowArgumentOutOfRangeException(byte customerLevelValue)
        {
            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam(1, CustomerLevel.Super, 1, (CustomerLevel)customerLevelValue)));
        }
        
        [Test]
        public void SetUplineLevel_ValueIsNullWhenCustomerIsMember_ThrowArgumentOutOfRangeException()
        {
            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam(1, CustomerLevel.Member, 1, null)));
        }
    }
}