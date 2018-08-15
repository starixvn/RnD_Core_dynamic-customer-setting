namespace Fanex.CustomerSetting.SettingType5.Tests.Models
{
    using CustomerSetting.Models.Constants;
    using CustomerSetting.Models.Enumerations;
    using NUnit.Framework;
    using SettingType5.Models;

    [TestFixture]
    public class ChargedCommissionSettingTypeTests
    {
        private static readonly object[] notMemberCustomers =
        {
            new object[] { Level.Super },
            new object[] { Level.Master },
            new object[] { Level.Agent }
        };

        private ChargedCommissionSettingType _settingType;

        [SetUp]
        public void Setup()
        {
            _settingType = new ChargedCommissionSettingType
            {
                DisplayMode = DisplayModes.Editable
            };
        }

        [Test, TestCaseSource(nameof(notMemberCustomers))]
        public void SatisfyCustomerLevelConstraint_ActorIsAnyLevelAndCustomerIsNotMember_ReturnTrue(Level customerLevel)
        {
            // Arrange.
            var actorLevel = Level.Company;

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_ActorIsAnyLevelAndCustomerIsMember_ReturnFalse()
        {
            // Arrange.
            var actorLevel = Level.Company;
            var customerLevel = Level.Member;

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsFalse(actualResult);
        }
    }
}