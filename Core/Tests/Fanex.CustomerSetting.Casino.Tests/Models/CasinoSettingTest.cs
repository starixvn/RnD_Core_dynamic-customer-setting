namespace Fanex.CustomerSetting.Casino.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using Casino.Models;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CasinoSettingTest
    {
        private ISetting setting;

        [SetUp]
        public void Init()
        {
            setting = new Setting();
            setting.CustomerId = 1;
            setting.CustomerLevel = Level.Member;
            setting.ProductId = 1;
            setting.SettingTypes = new List<SettingType>();
        }

        [Test]
        public void CasinoSettingCtor_SettingIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CasinoSetting(null, Arg.Any<bool>()));
        }

        [Test]
        public void CasinoSetting_CustomerIsSynced_IsNormalModeMustBeTrue()
        {
            // Arrange.
            bool syncedStatus = true;
            var casinoSetting = new CasinoSetting(setting, syncedStatus);

            //// Act
            var actual = casinoSetting.IsNormalMode;

            // Assert.
            Assert.IsTrue(actual);
        }

        [Test]
        public void CasinoSetting_CustomerProductStatuIsUnsynced_IsNormalModeMustBeFalse()
        {
            // Arrange.
            bool syncedStatus = false;
            var casinotSetting = new CasinoSetting(this.setting, syncedStatus);

            // Act.
            var actual = casinotSetting.IsNormalMode;

            // Assert.
            Assert.IsFalse(actual);
        }
    }
}