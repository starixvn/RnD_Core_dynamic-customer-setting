namespace Fanex.CustomerSetting.Tests.Dtos.Params
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Dtos.Params;
    using NUnit.Framework;

    [TestFixture]
    public class UpdatingSettingParamTests
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void UpdateCustomerSetting_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () =>
                    {
                        updatingParam.Setting.CustomerId = customerId;
                    }));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateUpdatingSetting_CustomerIdIsNegativeOrZero_ReturnValidationError(int customerId)
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => InstanceCreator.CreateUpdatingSettingParam(customerId)));
        }
    }
}
