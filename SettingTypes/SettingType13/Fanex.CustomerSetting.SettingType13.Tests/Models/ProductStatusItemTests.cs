namespace Fanex.CustomerSetting.SettingType13.Tests.Models
{
    using System.Collections;
    using Dtos;
    using NUnit.Framework;
    using SettingType13.Models;

    public class ProductStatusItemTests
    {
        private static IEnumerable SampleProductStatusForValidateMethod
        {
            get
            {
                yield return new TestCaseData(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = true,
                        Downline = new ProductStatusItem(false)
                    }).Returns(9101);
                yield return new TestCaseData(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = true,
                        Downline = new ProductStatusItem(true)
                    }).Returns(0);
                yield return new TestCaseData(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = true
                    }).Returns(0);
                yield return new TestCaseData(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = false,
                        Downline = new ProductStatusItem(true)
                    }).Returns(0);
                yield return new TestCaseData(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = false,
                        Downline = new ProductStatusItem(false)
                    }).Returns(0);
            }
        }

        private static IEnumerable SampleProductStatusForToXmlMethod
        {
            get
            {
                yield return new TestCaseData(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = true,
                        Downline = new ProductStatusItem(true)
                    }).Returns("<ProductStatus SportType=\"1\" BetType=\"1\" IsDisabled=\"1\" IsDisabledDownline=\"1\"/>");
                yield return new TestCaseData(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = false,
                        Downline = new ProductStatusItem(false)
                    }).Returns("<ProductStatus SportType=\"1\" BetType=\"1\" IsDisabled=\"0\" IsDisabledDownline=\"0\"/>");
                yield return new TestCaseData(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = true
                    }).Returns("<ProductStatus SportType=\"1\" BetType=\"1\" IsDisabled=\"1\" IsDisabledDownline=\"0\"/>");
                yield return new TestCaseData(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = true,
                        Downline = new ProductStatusItem(false)
                    }).Returns("<ProductStatus SportType=\"1\" BetType=\"1\" IsDisabled=\"1\" IsDisabledDownline=\"0\"/>");
            }
        }

        private static IEnumerable SampleOdProductStatusDtos
        {
            get
            {
                yield return new TestCaseData(
                    new ProductStatusDto
                    {
                        BetTypeId = 1,
                        SportTypeId = 1,
                        IsDisabled = true,
                        IsDisabledDownline = null,
                        IsDisabledUpline = null,
                        HasDownline = true
                    }).Returns(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = true
                    });
                yield return new TestCaseData(
                    new ProductStatusDto
                    {
                        BetTypeId = 1,
                        SportTypeId = 1,
                        IsDisabled = true,
                        IsDisabledDownline = false,
                        IsDisabledUpline = false,
                        HasDownline = true
                    }).Returns(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = true,
                        Upline = new ProductStatusItem(false),
                        Downline = new ProductStatusItem(false)
                    });
                yield return new TestCaseData(
                    new ProductStatusDto
                    {
                        BetTypeId = 1,
                        SportTypeId = 1,
                        IsDisabled = true,
                        IsDisabledDownline = null,
                        IsDisabledUpline = true,
                        HasDownline = true
                    }).Returns(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = true,
                        Upline = new ProductStatusItem(true)
                    });
                yield return new TestCaseData(
                    new ProductStatusDto
                    {
                        BetTypeId = 1,
                        SportTypeId = 1,
                        IsDisabled = true,
                        IsDisabledDownline = true,
                        IsDisabledUpline = true,
                        HasDownline = false
                    }).Returns(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = true,
                        Upline = new ProductStatusItem(true)
                    });
            }
        }

        [Test, TestCaseSource(nameof(SampleProductStatusForValidateMethod))]
        public int ValidateMethodReturnsExpectedErrorCode(ProductStatusItem productStatusItem)
        {
            var actualResult = productStatusItem.Validate();

            return actualResult.Code;
        }

        [Test, TestCaseSource(nameof(SampleProductStatusForToXmlMethod))]
        public string ToXmlMethodReturnsExpectedXmlString(ProductStatusItem productStatusItem)
            => productStatusItem.ToXml();

        [Test, TestCaseSource(nameof(SampleOdProductStatusDtos))]
        public ProductStatusItem ConstructorInstantiateDataFromDto(ProductStatusDto dto)
            => new ProductStatusItem(dto);
    }
}