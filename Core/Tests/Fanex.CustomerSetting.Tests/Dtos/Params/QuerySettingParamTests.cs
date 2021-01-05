namespace Fanex.CustomerSetting.Tests.Dtos.Params
{
    using System;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using NUnit.Framework;

    [TestFixture]
    public class QuerySettingParamTests
    {
        private static readonly object[] dataSourceSMALevels = { Level.Super, Level.Master, Level.Agent };

        [TestCase(0)]
        [TestCase(-1)]
        public void SetCustomerId_ValueIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new CustomerSettingQuery(customerId, Level.Master, 0, 0, null)));
        }

        [Test]
        public void SetProductId_ValueIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new CustomerSettingQuery(1, Level.Master, 0, 0, null)));
        }

        [Test, TestCaseSource(nameof(dataSourceSMALevels))]
        public void SetUplineLevel_ValueIsNotNullAndCustomerIsNotMember_ThrowArgumentOutOfRangeException(Level customerLevel)
        {
            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new CustomerSettingQuery(1, customerLevel, 1, 0, customerLevel)));
        }

        [Test]
        public void SetUplineLevel_ValueIsNullAndCustomerIsMember_ThrowArgumentNullException()
        {
            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => new CustomerSettingQuery(1, Level.Member, 1, 0, null)));
        }
    }
}