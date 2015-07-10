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
        public void UpdateMultipleCustomerSetting_NumberOfCustomerIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceCreator.CreateUpdatingMultipleSettingParam();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => updatingMultipleSettingParam.CustomerIds = new List<int>()));
        }
    }
}