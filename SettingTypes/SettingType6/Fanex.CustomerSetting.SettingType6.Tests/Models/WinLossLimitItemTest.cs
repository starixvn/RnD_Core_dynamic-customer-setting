namespace Fanex.CustomerSetting.SettingType6.Tests.Models
{
    using Constants;
    using Fanex.CustomerSetting.SettingType6.Models;
    using Fanex.CustomerSetting.SettingType6.Resources;
    using Fanex.CustomerSetting.SettingType6.Utilities;
    using NUnit.Framework;

    public class WinLossLimitItemTest
    {
        [Test]
        public void Validate_Always_ValidationResultIsNotNull()
        {
            // Arrange
            var winLossLimit = new WinLossLimitItem();

            // Act
            var validationResult = winLossLimit.Validate();

            // Assert
            Assert.IsNotNull(validationResult);
        }

        [Test]
        public void Validate_WinLimitIsLessThanZero_IsFailed()
        {
            // Arrange
            var winLossLimit = new WinLossLimitItem();
            winLossLimit.WinLimit = -1;

            // Act
            var validationResult = winLossLimit.Validate();

            // Assert
            Assert.IsTrue(validationResult.IsFailed);
        }

        [Test]
        public void Validate_WinLimitIsLessThanZero_ReturnMaximumWinMustNotBeLessThanZeroErrorCode()
        {
            // Arrange
            var winLossLimit = new WinLossLimitItem();
            winLossLimit.WinLimit = -1;

            // Act
            var validationResult = winLossLimit.Validate();

            // Assert
            Assert.AreEqual(ErrorCode.MaximumWinMustNotBeLessThanZero, validationResult.Code);
        }

        [Test]
        public void Validate_WinLimitIsLessThanZero_ReturnMaximumWinMustNotBeLessThanZeroError()
        {
            // Arrange
            var winLossLimit = new WinLossLimitItem();
            winLossLimit.WinLimit = -1;

            // Act
            var validationResult = winLossLimit.Validate();

            // Assert
            Assert.AreEqual(Message.MaximumWinShouldNotBeLessThanZero, validationResult.Message);
        }

        [Test]
        public void Validate_WinLimitIsGreaterThanMaxWinRange_IsFailed()
        {
            // Arrange
            var winLossLimit = new WinLossLimitItem();
            winLossLimit.WinLimit = 100;
            winLossLimit.MaxWinRange = 99;

            // Act
            var validationResult = winLossLimit.Validate();

            // Assert
            Assert.IsTrue(validationResult.IsFailed);
        }

        [Test]
        public void Validate_WinLimitIsGreaterThanMaxWinRange_ReturnMaximumWinMustBotExceedThanUplineMaximumWinErrorCode()
        {
            // Arrange
            var winLossLimit = new WinLossLimitItem();
            winLossLimit.WinLimit = 100;
            winLossLimit.MaxWinRange = 99;

            // Act
            var validationResult = winLossLimit.Validate();

            // Assert
            Assert.AreEqual(ErrorCode.MaximumWinMustBotExceedThanUplineMaximumWin, validationResult.Code);
        }

        [Test]
        public void Validate_WinLimitIsGreaterThanMaxWinRange_ReturnMaximumWinMustBotExceedThanUplineMaximumWinError()
        {
            // Arrange
            var winLossLimit = new WinLossLimitItem();
            winLossLimit.WinLimit = 100;
            winLossLimit.MaxWinRange = 99;

            // Act
            var validationResult = winLossLimit.Validate();

            // Assert
            Assert.AreEqual(Message.MaximumWinShouldBotExceedThanUplineMaximumWin, validationResult.Message);
        }

        [TestCase(100, 99, 90)]
        [TestCase(80, 99, 90)]
        public void Validate_MinLossRangeIsOutOfRange_IsFailed(decimal lossLimit, decimal maxLossRange, decimal minLossRange)
        {
            // Arrange
            var winLossLimit = new WinLossLimitItem();
            winLossLimit.LossLimit = lossLimit;
            winLossLimit.MaxLossRange = maxLossRange;
            winLossLimit.MinLossRange = minLossRange;

            // Act
            var validationResult = winLossLimit.Validate();

            // Assert
            Assert.IsTrue(validationResult.IsFailed);
        }

        [TestCase(100, 99, 90)]
        [TestCase(80, 99, 90)]
        public void Validate_MinLossRangeIsOutOfRange_ReturnMaximumLossValueMustBeInRangeErrorCode(
            decimal lossLimit,
            decimal maxLossRange,
            decimal minLossRange)
        {
            // Arrange
            var winLossLimit = new WinLossLimitItem();
            winLossLimit.LossLimit = lossLimit;
            winLossLimit.MaxLossRange = maxLossRange;
            winLossLimit.MinLossRange = minLossRange;

            // Act
            var validationResult = winLossLimit.Validate();

            // Assert
            Assert.AreEqual(ErrorCode.MaximumLossValueMustBeInRange, validationResult.Code);
        }

        [TestCase(100, 99, 90)]
        [TestCase(80, 99, 90)]
        public void Validate_MinLossRangeIsOutOfRange_ReturnMaximumLossValueMustBeInRangeError(
            decimal lossLimit,
            decimal maxLossRange,
            decimal minLossRange)
        {
            // Arrange
            var winLossLimit = new WinLossLimitItem();
            winLossLimit.LossLimit = lossLimit;
            winLossLimit.MaxLossRange = maxLossRange;
            winLossLimit.MinLossRange = minLossRange;

            // Act
            var validationResult = winLossLimit.Validate();

            // Assert
            string expectedMessage = string.Format(
                Message.MaximumLossValueShouldBeInRange,
                NumberFormatter.FormatDecimal(minLossRange),
                NumberFormatter.FormatDecimal(maxLossRange));
            Assert.AreEqual(expectedMessage, validationResult.Message);
        }

        [Test]
        public void ToXml_Always_ReturnValidXmlString()
        {
            // Arrange
            var winLossLimit = new WinLossLimitItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                WinLimit = 2,
                LossLimit = 200,
                IsAutoReset = false
            };

            // Act
            var actualXml = winLossLimit.ToXml();

            // Assert
            var expectedXml = "<Limit SportType=\"1\" BetType=\"1\" MaxWinLimit=\"2\" MaxLossLimit=\"200\" IsAutoReset=\"0\" />";
            Assert.AreEqual(expectedXml, actualXml);
        }
    }
}