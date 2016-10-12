namespace Fanex.CustomerSetting.SettingType12.Tests.Models
{
    using Constants;
    using NUnit.Framework;
    using Resources;
    using SettingType12.Models;
    using Utilities;

    public class LossLimitItemTest
    {
        [Test]
        public void Validate_Always_ValidationResultIsNotNull()
        {
            // Arrange
            var lossLimitItem = new LossLimitItem();

            // Act
            var validationResult = lossLimitItem.Validate();

            // Assert
            Assert.IsNotNull(validationResult);
        }

        [TestCase(100, 99, 90)]
        [TestCase(80, 99, 90)]
        public void Validate_MinLossRangeIsOutOfRange_IsFailed(decimal lossLimit, decimal maxLossRange, decimal minLossRange)
        {
            // Arrange
            var lossLimitItem = new LossLimitItem();

            lossLimitItem.LossLimit = lossLimit;
            lossLimitItem.MaxLossRange = maxLossRange;
            lossLimitItem.MinLossRange = minLossRange;

            // Act
            var validationResult = lossLimitItem.Validate();

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
            var lossLimitItem = new LossLimitItem();

            lossLimitItem.LossLimit = lossLimit;
            lossLimitItem.MaxLossRange = maxLossRange;
            lossLimitItem.MinLossRange = minLossRange;

            // Act
            var validationResult = lossLimitItem.Validate();

            // Assert
            Assert.AreEqual(ErrorCode.MaximumLossValueShouldBeInRange, validationResult.Code);
        }

        [TestCase(100, 99, 90)]
        [TestCase(80, 99, 90)]
        public void Validate_MinLossRangeIsOutOfRange_ReturnMaximumLossValueMustBeInRangeError(
            decimal lossLimit,
            decimal maxLossRange,
            decimal minLossRange)
        {
            // Arrange
            var lossLimitItem = new LossLimitItem();

            lossLimitItem.LossLimit = lossLimit;
            lossLimitItem.MaxLossRange = maxLossRange;
            lossLimitItem.MinLossRange = minLossRange;

            // Act
            var validationResult = lossLimitItem.Validate();

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
            var lossLimitItem = new LossLimitItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                LossLimit = 200,
                IsAutoReset = false
            };

            // Act
            var actualXml = lossLimitItem.ToXml();

            // Assert
            var expectedXml = "<Limit SportType=\"1\" BetType=\"1\" MaxLossLimit=\"200\" IsAutoReset=\"0\" />";
            Assert.AreEqual(expectedXml, actualXml);
        }
    }
}