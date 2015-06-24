namespace Fanex.CustomerSetting.SettingType4.Template1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType4.Template1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class ProductStatusItemTests
    {
        private ProductStatusItem _productStatusItem;

        [SetUp]
        public void Setup()
        {
            _productStatusItem = new ProductStatusItem
            {
                SportTypeId = 1,
                BetTypeId = 1
            };
        }

        [Test]
        public void Validate_DownlineProductStatusIsNull_ReturnErrorCode()
        {
            //// Arrange
            _productStatusItem.IsDisabled = true;
            _productStatusItem.Downline = null;

            //// Act
            var actualResult = _productStatusItem.Validate();

            //// Assert
            Assert.AreEqual(9102, actualResult.Code);
        }

        [Test]
        public void Validate_CurrentStatusIsDisabledAndDownlineStatusIsEnabled_ReturnErrorCode()
        {
            //// Arrange
            _productStatusItem.IsDisabled = true;
            _productStatusItem.Downline = new ProductStatusItem { IsDisabled = false };

            //// Act
            var actualResult = _productStatusItem.Validate();

            //// Assert
            Assert.AreEqual(9101, actualResult.Code);
        }

        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            _productStatusItem = new ProductStatusItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                IsDisabled = true,
                Downline = new ProductStatusItem
                {
                    IsDisabled = true
                }
            };

            //// Act
            var actualXmlFormatted = _productStatusItem.ToXml();

            //// Assert
            var expectedXmlFormatted = "<ProductStatus SportType=\"1\" BetType=\"1\" IsDisabled=\"1\" IsDisabledDownline=\"1\"/>";
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}