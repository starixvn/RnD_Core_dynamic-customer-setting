namespace Fanex.CustomerSetting.Tests.Dtos.Params
{
    using System;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class QuerySettingParamTest
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void GetCustomerSetting_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam
                    {
                        CustomerId = customerId
                    }));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateQuerySetting_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam
                    {
                        CustomerId = customerId
                    }));
        }

        [Test]
        public void GetCustomerSetting_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam
                    {
                        ProductId = 0
                    }));
        }

        [Test]
        public void GetCustomerSetting_CustomerLevelValueIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam
                    {
                        CustomerLevel = (CustomerLevel)0
                    }));
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateQuerySetting_CompanyEditsCustomerIsNotMemberAndUplineLevelHasValue_ReturnAccessDeniedResult(CustomerLevel customerLevel)
        {
            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => new QuerySettingParam
                    {
                        CustomerId = 1,
                        CustomerLevel = customerLevel,
                        UplineLevel = customerLevel
                    }));
        }
    }
}