namespace Fanex.CustomerSetting.SettingType15.Tests.Models
{
    using CustomerSetting.Models.Constants;
    using CustomerSetting.Models.Enumerations;
    using NUnit.Framework;
    using SettingType15.Models;

    [TestFixture]
    public class WinningShareSettingTypeTests
    {
        private WinningShareSettingType _settingType;

        [SetUp]
        public void Setup()
        {
            _settingType = new WinningShareSettingType
            {
                DisplayMode = DisplayModes.Editable
            };
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_Always_ReturnFalse()
        {
            // Arrange.
            var anyActorLevel = Level.Company;
            var anyCustomerLevel = Level.Super;

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(anyActorLevel, anyCustomerLevel);

            // Assert.
            Assert.IsFalse(actualResult);
        }
    }
}