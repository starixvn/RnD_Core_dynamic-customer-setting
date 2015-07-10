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
        public void SetCustomerIds_NumberOfCustomerIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => updatingMultipleSettingParam.CustomerIds = new List<int>()));
        }

        [Test]
        public void SetCustomerNames_NumberOfCustomerNameIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => updatingMultipleSettingParam.CustomerNames = new List<string>()));
        }

        [Test]
        public void SetCustomerNames_CustomerIdsListDoesNotMatchWithCustomerNameList_ThrowArgumentOutOfRangeException()
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
    }
}