namespace Fanex.CustomerSetting.SettingType11.Tests.Models
{
    using System.Collections;
    using Dtos;
    using NUnit.Framework;
    using SettingType11.Models;

    public class ProductStatusItemTests
    {
        private static IEnumerable SampleProductStatusForToXmlMethod
        {
            get
            {
                yield return new TestCaseData(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = true
                    }).Returns("<ProductStatus SportType=\"1\" BetType=\"1\" IsDisabled=\"1\"/>");
                yield return new TestCaseData(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = false
                    }).Returns("<ProductStatus SportType=\"1\" BetType=\"1\" IsDisabled=\"0\"/>");
            }
        }

        private static IEnumerable SampleOfProductStatusDtos
        {
            get
            {
                yield return new TestCaseData(
                    new ProductStatusDto
                    {
                        BetTypeId = 1,
                        SportTypeId = 1,
                        IsDisabled = true,
                        IsDisabledUpline = null
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
                        IsDisabledUpline = false
                    }).Returns(
                    new ProductStatusItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        IsDisabled = true,
                        Upline = new ProductStatusItem(false)
                    });
                yield return new TestCaseData(
                    new ProductStatusDto
                    {
                        BetTypeId = 1,
                        SportTypeId = 1,
                        IsDisabled = true,
                        IsDisabledUpline = true
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

        [Test, TestCaseSource(nameof(SampleProductStatusForToXmlMethod))]
        public string ToXmlMethodReturnsExpectedXmlString(ProductStatusItem productStatusItem)
            => productStatusItem.ToXml();

        [Test, TestCaseSource(nameof(SampleOfProductStatusDtos))]
        public ProductStatusItem ConstructorInstantiateDataFromDto(ProductStatusDto dto)
            => new ProductStatusItem(dto);
    }
}