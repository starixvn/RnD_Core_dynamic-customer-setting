namespace Fanex.CustomerSetting.Tests.Dtos.Params
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using NUnit.Framework;

    [TestFixture]
    public class UpdatingSettingParamTests
    {
        private object[] dataSourceSMALevels = { Level.Super, Level.Master, Level.Agent };

        [Test, TestCaseSource("dataSourceSMALevels")]
        public void SetUplineLevel_ValueIsNotNullAndCustomerIsNotMember_ThrowArgumentOutOfRangeException(Level customerLevel)
        {
            var setting = new Setting(
                customerId: 1,
                customerLevel: customerLevel,
                productId: 1,
                settingTypes: new List<SettingType>());
            var updateSettingParam = new UpdatingSettingParam(setting);

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => updateSettingParam.UplineLevel = customerLevel));
        }

        [Test]
        public void SetUplineLevel_ValueIsNullAndCustomerIsMember_ThrowArgumentOutOfRangeException()
        {
            var setting = new Setting(
                customerId: 1,
                customerLevel: Level.Member,
                productId: 1,
                settingTypes: new List<SettingType>());

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => new UpdatingSettingParam(setting)));
        }

        [Test]
        public void SetSetting_SettingIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => updatingParam.Setting = null));
        }
    }
}