namespace Fanex.CustomerSetting.Tests.Dtos.Params
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Params;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateMultipleSettingParamTests
    {
        [Test]
        public void SetCustomerIds_CustomerIdListIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => updateMultipleSettingParam.CustomerIds = null));
        }

        [Test]
        public void SetCustomerIds_CustomerIdListHasNoItemWhenCustomerNameListHasItem_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => updateMultipleSettingParam.CustomerIds = new List<int>()));
        }

        [Test]
        public void SetCustomerNames_CustomerNameListIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => updateMultipleSettingParam.CustomerNames = null));
        }

        [Test]
        public void SetCustomerNames_CustomerNameListHasNoItemWhenCustomerIdListHasItems_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => updateMultipleSettingParam.CustomerNames = new List<string>()));
        }

        [Test]
        public void SetCustomerNames_CustomerIdListDoesNotMatchWithCustomerNameList_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () =>
                    {
                        updateMultipleSettingParam.CustomerIds = new List<int> { 1, 2 };
                        updateMultipleSettingParam.CustomerNames = new List<string> { "Username1" };
                    }));
        }

        [Test]
        public void SetSetting_SettingIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => updateMultipleSettingParam.Setting = null));
        }
    }
}