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
        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void SetUplineLevel_ValueIsNotNullAndCustomerIsNotMember_ThrowArgumentOutOfRangeException(CustomerLevel customerLevel)
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

        [TestCase(5)]
        [TestCase(10)]
        public void SetUplineLevel_ValueIsGreaterThanSuperLevelValue_ThrowArgumentOutOfRangeException(byte uplineLevelValue)
        {
            var setting = new Setting(
                customerId: 1,
                customerLevel: CustomerLevel.Member,
                productId: 1,
                settingTypes: new List<SettingType>());

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new UpdatingSettingParam(setting, (CustomerLevel)uplineLevelValue)));
        }

        [Test]
        public void SetUplineLevel_ValueIsNullAndCustomerIsMember_ThrowArgumentOutOfRangeException()
        {
            var setting = new Setting(
                customerId: 1,
                customerLevel: CustomerLevel.Member,
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