namespace Fanex.CustomerSetting.Tests.Models
{
    using System;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using NUnit.Framework;

    [TestFixture]
    public class UserTest
    {
        private static object[] isMethodAlwaysReturnRightValueCases =  
        {
            new object[] { UserLevel.Company, UserLevel.Company, true },
            new object[] { UserLevel.Company, UserLevel.Super, false },
            new object[] { UserLevel.Super, UserLevel.Super, true },
            new object[] { UserLevel.Super, UserLevel.Master, false },
            new object[] { UserLevel.Master, UserLevel.Master, true },
            new object[] { UserLevel.Master, UserLevel.Agent, false },
            new object[] { UserLevel.Agent, UserLevel.Agent, true }
        };

        [TestCase(0)]
        [TestCase(-1)]
        public void SetId_InputValueIsNegativeOrZero_ThrowArgumentOutOfRangeException(int userId)
        {
            //// Arrange
            User user = InstanceFactory.CreateUser();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () =>
                    {
                        user.Id = userId;
                    }));
        }

        [TestCase("    ")]
        [TestCase("")]
        [TestCase(null)]
        public void SetName_InputValueIsNullOrWhiteSpace_ThrowArgumentOutOfRangeException(string userName)
        {
            //// Arrange
            User user = InstanceFactory.CreateUser();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () =>
                    {
                        user.Name = userName;
                    }));
        }

        [Test]
        public void SetLevel_InputValuesIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            User user = InstanceFactory.CreateUser();

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () =>
                    {
                        user.Level = null;
                    }));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        public void SetSubAccountId_InputValueIsNegative_ThrowArgumentOutOfRangeException(int subAccountId)
        {
            //// Arrange
            User user = InstanceFactory.CreateUser();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () =>
                    {
                        user.SubAccountId = subAccountId;
                    }));
        }
    }
}