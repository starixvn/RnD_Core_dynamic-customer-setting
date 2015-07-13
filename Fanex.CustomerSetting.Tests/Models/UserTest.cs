namespace Fanex.CustomerSetting.Tests.Models
{
    using System;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using NUnit.Framework;

    [TestFixture]
    public class UserTest
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void SetId_ValueIsNegativeOrZero_ThrowArgumentOutOfRangeException(int userId)
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

        [TestCase("")]
        [TestCase(null)]
        public void SetName_ValueIsNullOrEmpty_ThrowArgumentOutOfRangeException(string userName)
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

        [TestCase(0)]
        [TestCase(6)]
        [TestCase(100)]
        public void SetLevel_ValueIsOutOfLevelRange_ThrowArgumentOutOfRangeException(int levelValue)
        {
            //// Arrange
            User user = InstanceFactory.CreateUser();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () =>
                    {
                        user.Level = Enumeration.FromValue<UserLevel>(levelValue);
                    }));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        public void SetSubAccountId_ValueIsNegative_ThrowArgumentOutOfRangeException(int subAccountId)
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