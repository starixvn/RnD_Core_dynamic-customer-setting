namespace Fanex.CustomerSetting.SettingType4.Tests.Models
{
    using Fanex.CustomerSetting.SettingType4.Dtos;
    using Fanex.CustomerSetting.SettingType4.Models;
    using NUnit.Framework;

    [TestFixture]
    public class ProductStatusItemTests
    {
        [TestCase(true, false, 9101)]
        [TestCase(true, true, 0)]
        [TestCase(true, null, 0)]
        [TestCase(false, true, 0)]
        [TestCase(false, false, 0)]
        public void Validate_CurrentStatusAndDownlineStatus_ReturnErrorCode(
            bool isDisabled,
            bool? isDisabledDowline,
            int errorCode)
        {
            //// Arrange
            var productStatusItem = new ProductStatusItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                IsDisabled = isDisabled
            };

            if (isDisabledDowline != null)
            {
                productStatusItem.Downline = new ProductStatusItem { IsDisabled = isDisabledDowline.Value };
            }

            //// Act
            var actualResult = productStatusItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [TestCase(true, true, "<ProductStatus SportType=\"1\" BetType=\"1\" IsDisabled=\"1\" IsDisabledDownline=\"1\"/>")]
        [TestCase(false, false, "<ProductStatus SportType=\"1\" BetType=\"1\" IsDisabled=\"0\" IsDisabledDownline=\"0\"/>")]
        [TestCase(true, false, "<ProductStatus SportType=\"1\" BetType=\"1\" IsDisabled=\"1\" IsDisabledDownline=\"0\"/>")]
        [TestCase(true, null, "<ProductStatus SportType=\"1\" BetType=\"1\" IsDisabled=\"1\" IsDisabledDownline=\"0\"/>")]
        public void ToXml_ReturnFormattedXml(
            bool isDisabled,
            bool? isDownlineDisbaled,
            string expectedXmlFormatted)
        {
            //// Arrange
            var productStatusItem = new ProductStatusItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                IsDisabled = isDisabled
            };

            if (isDownlineDisbaled != null)
            {
                productStatusItem.Downline = new ProductStatusItem { IsDisabled = isDownlineDisbaled.Value };
            }

            //// Act
            var actualXmlFormatted = productStatusItem.ToXml();

            //// Assert
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }

        [TestCase(true, null, null)]
        [TestCase(true, false, true)]
        [TestCase(true, false, true)]
        [TestCase(false, false, false)]
        public void Constructor_PopulatesDataFromProductStatusItemDto(
            bool isDisbaled,
            bool? isDisabledDownline,
            bool? isDisabledUpline)
        {
            //// Arrange
            var productStatusItemDto = new ProductStatusItemDto
            {
                BetTypeId = 1,
                SportTypeId = 1,
                IsDisabled = isDisbaled,
                IsDisabledDownline = isDisabledDownline,
                IsDisabledUpline = isDisabledUpline,
                HasDownline = true
            };

            //// Act
            var actualProductStatusItem = new ProductStatusItem(productStatusItemDto);

            //// Assert
            Assert.AreEqual(1, actualProductStatusItem.SportTypeId);
            Assert.AreEqual(1, actualProductStatusItem.BetTypeId);
            Assert.AreEqual(isDisbaled, actualProductStatusItem.IsDisabled);

            if (isDisabledDownline == null)
            {
                Assert.IsNull(actualProductStatusItem.Downline);
            }
            else
            {
                Assert.AreEqual(isDisabledDownline.Value, actualProductStatusItem.Downline.IsDisabled);
            }

            if (isDisabledUpline == null)
            {
                Assert.IsNull(actualProductStatusItem.Upline);
            }
            else
            {
                Assert.AreEqual(isDisabledUpline.Value, actualProductStatusItem.Upline.IsDisabled);
            }
        }

        [Test]
        public void Constructor_CustomerHasNoDownLine_DownlineIsNull()
        {
            //// Arrange
            var productStatusItemDto = new ProductStatusItemDto
            {
                BetTypeId = 1,
                SportTypeId = 1
            };

            //// Act
            var actualProductStatusItem = new ProductStatusItem(productStatusItemDto);

            //// Assert
            Assert.IsNull(actualProductStatusItem.Downline);
        }
    }
}