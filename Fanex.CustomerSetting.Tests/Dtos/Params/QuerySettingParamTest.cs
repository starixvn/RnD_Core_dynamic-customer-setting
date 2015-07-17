namespace Fanex.CustomerSetting.Tests.Dtos.Params
{
    using System;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class QuerySettingParamTest
    {
        private object[] dataSourceSMALevels = { UserLevel.Super, UserLevel.Master, UserLevel.Agent };

        [TestCase(0)]
        [TestCase(-1)]
        public void SetCustomerId_ValueIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam(0, UserLevel.Master, 0, null)));
        }

        [Test]
        public void SetProductId_ValueIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam(1, UserLevel.Master, 0, null)));
        }

        [Test, TestCaseSource("dataSourceSMALevels")]
        public void SetUplineLevel_ValueIsNotNullAndCustomerIsNotMember_ThrowArgumentOutOfRangeException(UserLevel customerLevel)
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
                    () => new QuerySettingParam(1, UserLevel.Member, 1, null)));
        }
    }
}