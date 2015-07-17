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
            new object[] { Level.Company, Level.Company, true },
            new object[] { Level.Company, Level.Super, false },
            new object[] { Level.Super, Level.Super, true },
            new object[] { Level.Super, Level.Master, false },
            new object[] { Level.Master, Level.Master, true },
            new object[] { Level.Master, Level.Agent, false },
            new object[] { Level.Agent, Level.Agent, true }
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

        [Test]
        public void SetLevel_InputValuesIsMember_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            User user = InstanceFactory.CreateUser();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () =>
                    {
                        user.Level = Level.Member;
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