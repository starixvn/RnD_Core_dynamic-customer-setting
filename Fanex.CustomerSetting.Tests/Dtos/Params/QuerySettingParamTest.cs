namespace Fanex.CustomerSetting.Tests.Dtos.Params
{
    using System;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class QuerySettingParamTest
    {
        private object[] dataSourceSMALevels = { Level.Super, Level.Master, Level.Agent };

        [TestCase(0)]
        [TestCase(-1)]
        public void SetCustomerId_ValueIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam(customerId, Level.Master, 0, null)));
        }

        [Test]
        public void SetProductId_ValueIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam(1, Level.Master, 0, null)));
        }

        [Test, TestCaseSource("dataSourceSMALevels")]
        public void SetUplineLevel_ValueIsNotNullAndCustomerIsNotMember_ThrowArgumentOutOfRangeException(Level customerLevel)
        {
            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam(1, customerLevel, 1, customerLevel)));
        }
        
        [Test]
        public void SetUplineLevel_ValueIsNullAndCustomerIsMember_ThrowArgumentNullException()
        {
            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => new QuerySettingParam(1, Level.Member, 1, null)));
        }
    }
}