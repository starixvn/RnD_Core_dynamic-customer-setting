namespace Fanex.CustomerSetting.Tests.Dtos.Params
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Requests;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateSettingParamTests
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
            var updateSettingParam = new UpdateCustomerSettingCommand(setting);

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
                    () => new UpdateCustomerSettingCommand(setting)));
        }

        [Test]
        public void SetSetting_SettingIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = InstanceFactory.CreateUpdateSettingParam();

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => updateParam.Setting = null));
        }
    }
}