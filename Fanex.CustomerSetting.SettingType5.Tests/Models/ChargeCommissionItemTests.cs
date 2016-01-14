namespace Fanex.CustomerSetting.SettingType5.Tests.Models
{
    using System;
    using Fanex.CustomerSetting.SettingType5.Dtos;
    using Fanex.CustomerSetting.SettingType5.Models;
    using NUnit.Framework;

    public class ChargeCommissionItemTests
    {
        [Test]
        public void SingleParameterContructor_ParameterIsNull_ThrowException()
        {
            // Arrange
            ChargeCommissionDto dto = null;

            // Act
            TestDelegate creatingObjDelegate = () => new ChargeCommissionItem(dto);

            // Assert
            Assert.Throws(typeof(ArgumentNullException), creatingObjDelegate);
        }

        [Test]
        public void SingleParameterContructor_ParameterIsNotNull_SportTypeIdIsDtoSportTypeId()
        {
            // Arrange
            var dto = new ChargeCommissionDto
            {
                SportTypeId = 1
            };

            // Act
            var chargeComm = new ChargeCommissionItem(dto);

            // Assert
            Assert.AreEqual(1, chargeComm.SportTypeId);
        }

        [Test]
        public void SingleParameterContructor_ParameterIsNotNull_BetTypeIdIsDtoBetTypeId()
        {
            // Arrange
            var dto = new ChargeCommissionDto
            {
                BetTypeId = 1
            };

            // Act
            var chargeComm = new ChargeCommissionItem(dto);

            // Assert
            Assert.AreEqual(1, chargeComm.BetTypeId);
        }

        [Test]
        public void SingleParameterContructor_ParameterIsNotNull_ChargeCommissionIsDtoChargeCommission()
        {
            // Arrange
            var dto = new ChargeCommissionDto
            {
                ChargeCommission = 0.88m
            };

            // Act
            var chargeComm = new ChargeCommissionItem(dto);

            // Assert
            Assert.AreEqual(0.88m, chargeComm.ChargeCommission);
        }

        [Test]
        public void SingleParameterContructor_ParameterIsNotNull_UplineIsNotNull()
        {
            // Arrange
            var dto = new ChargeCommissionDto();

            // Act
            var chargeComm = new ChargeCommissionItem(dto);

            // Assert
            Assert.IsNotNull(chargeComm.Upline);
        }

        [Test]
        public void SingleParameterContructor_ParameterIsNotNull_UplineObjChargeCommissionIsDtoUplineChargeCommission()
        {
            // Arrange
            var dto = new ChargeCommissionDto
            {
                UplineChargeCommission = 0.9m
            };

            // Act
            var chargeComm = new ChargeCommissionItem(dto);

            // Assert
            Assert.AreEqual(0.9m, chargeComm.Upline.ChargeCommission);
        }

        [Test]
        public void SingleParameterContructor_UplineOfUplineChargeCommissionIsNotNull_UplineObjOfUplineIsNotNull()
        {
            // Arrange
            var dto = new ChargeCommissionDto
            {
                UplineOfUplineChargeCommission = 0.9m
            };

            // Act
            var chargeComm = new ChargeCommissionItem(dto);

            // Assert
            Assert.IsNotNull(chargeComm.Upline.Upline);
        }

        [Test]
        public void SingleParameterContructor_UplineOfUplineChargeCommissionIsNotNull_ChargeCommissionOfUplineObjOfUplineProIsFromDtoObj()
        {
            // Arrange
            var dto = new ChargeCommissionDto
            {
                UplineOfUplineChargeCommission = 0.9m
            };

            // Act
            var chargeComm = new ChargeCommissionItem(dto);

            // Assert
            Assert.AreEqual(0.9m, chargeComm.Upline.Upline.ChargeCommission);
        }

        [Test]
        public void Validate_Always_ValidationResultIsNotNull()
        {
            // Arrange
            var chargeComm = new ChargeCommissionItem();

            // Act
            var validationResult = chargeComm.Validate();

            // Assert
            Assert.IsNotNull(validationResult);
        }

        [Test]
        public void Validate_UplineIsNull_IsFailed()
        {
            // Arrange
            var chargeComm = new ChargeCommissionItem();

            // Act
            var validationResult = chargeComm.Validate();

            // Assert
            Assert.IsTrue(validationResult.IsFailed);
        }

        [Test]
        public void Validate_ChargeCommissionIsLessThanZero_ReturnInvalidChargeCommissionErrorCode()
        {
            // Arrange
            var chargeComm = new ChargeCommissionItem
            {
                ChargeCommission = -1m
            };

            // Act
            var validationResult = chargeComm.Validate();

            // Assert
            int expectedErrorCode = 5102;
            Assert.AreEqual(expectedErrorCode, validationResult.Code);
        }

        [Test]
        public void Validate_ChargeCommissionIsGreaterThanOne_ReturnInvalidChargeCommissionErrorCode()
        {
            // Arrange
            var chargeComm = new ChargeCommissionItem
            {
                ChargeCommission = 1.5m
            };

            // Act
            var validationResult = chargeComm.Validate();

            // Assert
            int expectedErrorCode = 5101;
            Assert.AreEqual(expectedErrorCode, validationResult.Code);
        }

        [Test]
        public void Validate_UplineIsNull_ReturnInvalidChargeCommissionErrorCode()
        {
            // Arrange
            var chargeComm = new ChargeCommissionItem();

            // Act
            var validationResult = chargeComm.Validate();

            // Assert
            int expectedErrorCode = 5103;
            Assert.AreEqual(expectedErrorCode, validationResult.Code);
        }

        [Test]
        public void Validate_ChargeCommissionIsGreaterThanUplineChargeCommission_ReturnInvalidChargeCommissionErrorCode()
        {
            // Arrange
            var chargeComm = new ChargeCommissionItem
            {
                ChargeCommission = 0.9m,
                Upline = new ChargeCommissionItem()
                {
                    ChargeCommission = 0.88m
                }
            };

            // Act
            var validationResult = chargeComm.Validate();

            // Assert
            int expectedErrorCode = 5103;
            Assert.AreEqual(expectedErrorCode, validationResult.Code);
        }

        [Test]
        public void ToXml_Always_ReturnValidXmlString()
        {
            // Arrange
            var chargeComm = new ChargeCommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                ChargeCommission = 0.88m
            };

            // Act
            var actualXml = chargeComm.ToXml();

            // Assert
            var expectedXml = "<Discount SportType=\"1\" BetType=\"1\" Discount=\"0.88\"/>";
            Assert.AreEqual(expectedXml, actualXml);
        }
    }
}