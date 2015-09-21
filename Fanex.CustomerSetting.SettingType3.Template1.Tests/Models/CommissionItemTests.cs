namespace Fanex.CustomerSetting.SettingType3.Tests.Models
{
    using Fanex.CustomerSetting.SettingType3.Dtos;
    using Fanex.CustomerSetting.SettingType3.Models;
    using NUnit.Framework;

    [TestFixture]
    public class CommissionItemTests
    {
        [TestCase(-1, 5101)]
        [TestCase(-10, 5101)]
        [TestCase(1.1, 5102)]
        [TestCase(0.9, 0)]
        public void Validate_InValidCommission_ReturnErrorCode(
            decimal commissionRate,
            int errorCode)
        {
            //// Arrange
            var commissionItem = new CommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                CommRate = commissionRate
            };

            //// Act
            var actualResult = commissionItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [TestCase(1, 1, 0.12, "<Discount SportType=\"1\" BetType=\"1\" Discount=\"0.12\"/>")]
        [TestCase(1, 1, 0, "<Discount SportType=\"1\" BetType=\"1\" Discount=\"0\"/>")]
        [TestCase(10, 10, 0.0112, "<Discount SportType=\"10\" BetType=\"10\" Discount=\"0.0112\"/>")]
        public void ToXml_ReturnFormattedXml(
            short sportTypeId,
            short betTypeId,
            decimal commRate,
            string expectedXmlFormatted)
        {
            //// Arrange
            var commissionItem = new CommissionItem
            {
                SportTypeId = sportTypeId,
                BetTypeId = betTypeId,
                CommRate = commRate
            };

            //// Act
            var actualXmlFormatted = commissionItem.ToXml();

            //// Assert
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }

        [TestCase(0.11, 0.52, null)]
        [TestCase(0.11, 0.52, 0.66)]
        public void Constructor_PopulatesDataFromCommissionItemDto(
            decimal commRate,
            decimal uplineCommRate,
            double? uplineOfUplineCommRate)
        {
            //// Arrange
            var commissionItemDto = new CommissionItemDto
            {
                BetTypeId = 1,
                SportTypeId = 1,
                CommRate = commRate,
                UplineCommRate = uplineCommRate,
                UplineOfUplineCommRate = (decimal?)uplineOfUplineCommRate
            };

            //// Act
            var actualCommissionItem = new CommissionItem(commissionItemDto);

            //// Assert many times because this is same object.
            Assert.AreEqual(1, actualCommissionItem.SportTypeId);
            Assert.AreEqual(1, actualCommissionItem.BetTypeId);
            Assert.AreEqual(commRate, actualCommissionItem.CommRate);
            Assert.AreEqual(uplineCommRate, actualCommissionItem.Upline.CommRate);
            if (uplineOfUplineCommRate == null)
            {
                Assert.IsNull(actualCommissionItem.Upline.Upline);
            }
            else
            {
                Assert.AreEqual(uplineOfUplineCommRate.Value, actualCommissionItem.Upline.Upline.CommRate);
            }
        }
    }
}