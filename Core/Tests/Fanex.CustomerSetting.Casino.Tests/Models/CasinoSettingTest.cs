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
            Assert.Throws<ArgumentNullException>(() => new CasinoSetting(null, Arg.Any<CustomerProductStatus>()));
        }

        [Test]
        public void CasinoSettingCtor_CasinoStatusIsHidden_ThrowInvalidOperationException()
        {
            //// Arrange
            var customerProductStatus = new CustomerProductStatus { IsHidden = true };

            //// Act & Assert
            Assert.Throws<InvalidOperationException>(() => new CasinoSetting(setting, customerProductStatus));
        }

        [Test]
        public void CasinoSetting_CustomerProductStatusIsSyncedAndDonlineIsSynced_IsNormalModeMustBeTrue()
        {
            //// Arrange
            var customerProductStatus = new CustomerProductStatus { IsHidden = false, IsSynced = true, IsDownlineSynced = true };
            var casinotSetting = new CasinoSetting(this.setting, customerProductStatus);

            //// Act
            var actual = casinotSetting.IsNormalMode;

            //// Assert
            Assert.IsTrue(actual);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void CasinoSetting_CustomerProductStatusOrDonlineIsUnsynced_IsNormalModeMustBeFalsed(bool isSynced, bool isDownlineSynced)
        {
            //// Arrange
            var customerProductStatus = new CustomerProductStatus { IsHidden = false, IsSynced = isSynced, IsDownlineSynced = isDownlineSynced };
            var casinotSetting = new CasinoSetting(this.setting, customerProductStatus);

            //// Act
            var actual = casinotSetting.IsNormalMode;

            //// Assert
            Assert.IsFalse(actual);
        }
    }
}
