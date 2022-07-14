using System.Collections;
using Fanex.CustomerSetting.SettingType21.Dtos;
using Fanex.CustomerSetting.SettingType21.Models;
using NUnit.Framework;

namespace Fanex.CustomerSetting.SettingType21.Tests.Models
{
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

        [Test]
        [TestCaseSource(nameof(SampleProductStatusForToXmlMethod))]
        public string ToXmlMethodReturnsExpectedXmlString(ProductStatusItem productStatusItem)
        {
            return productStatusItem.ToXml();
        }

        [Test]
        [TestCaseSource(nameof(SampleOdProductStatusDtos))]
        public ProductStatusItem ConstructorInstantiateDataFromDto(ProductStatusDto dto)
        {
            return new ProductStatusItem(dto);
        }
    }
}