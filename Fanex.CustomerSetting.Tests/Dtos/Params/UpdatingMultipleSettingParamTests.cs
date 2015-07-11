namespace Fanex.CustomerSetting.Tests.Dtos.Params
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Params;
    using NUnit.Framework;

    [TestFixture]
    public class UpdatingMultipleSettingParamTests
    {
        [Test]
        public void SetCustomerIds_CustomerIdListIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => updatingMultipleSettingParam.CustomerIds = null));
        }

        [Test]
        public void SetCustomerIds_CustomerIdListHasNoItemWhenCustomerNameListHasItem_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => updatingMultipleSettingParam.CustomerIds = new List<int>()));
        }

        [Test]
        public void SetCustomerNames_CustomerNameListIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => updatingMultipleSettingParam.CustomerNames = null));
        }

        [Test]
        public void SetCustomerNames_CustomerNameListHasNoItemWhenCustomerIdListHasItems_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => updatingMultipleSettingParam.CustomerNames = new List<string>()));
        }

        [Test]
        public void SetCustomerNames_CustomerIdListDoesNotMatchWithCustomerNameList_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () =>
                    {
                        updatingMultipleSettingParam.CustomerIds = new List<int> { 1, 2 };
                        updatingMultipleSettingParam.CustomerNames = new List<string> { "Username1" };
                    }));
        }

        [Test]
        public void SetSetting_SettingIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => updatingMultipleSettingParam.Setting = null));
        }
    }
}