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
        public void SetUplineLevel_ValueIsNotNullWhenCustomerIsNotMember_ThrowArgumentOutOfRangeException(CustomerLevel customerLevel)
        {
            var setting = new Setting(1, customerLevel, 1, new List<SettingType>());
            var updateSettingParam = new UpdatingSettingParam(setting);

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => updateSettingParam.UplineLevel = customerLevel));
        }

        [TestCase(5)]
        [TestCase(10)]
        public void SetUplineLevel_ValueIsLargerThanSuperValue_ThrowArgumentOutOfRangeException(byte uplineLevelValue)
        {
            var setting = new Setting(1, CustomerLevel.Member, 1, new List<SettingType>());

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new UpdatingSettingParam(setting, (CustomerLevel)uplineLevelValue)));
        }

        [Test]
        public void SetUplineLevel_ValueIsNullWhenCustomerIsMember_ThrowArgumentOutOfRangeException()
        {
            var setting = new Setting(1, CustomerLevel.Member, 1, new List<SettingType>());

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new UpdatingSettingParam(setting)));
        }
    }
}