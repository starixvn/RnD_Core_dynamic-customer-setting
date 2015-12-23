namespace Fanex.CustomerSetting.SettingType7.Tests.Models
{
    using System;
    using Dtos;
    using NUnit.Framework;
    using SettingType7.Models;

    public class ChargedCommissionItemTests
    {
        [Test]
        public void SingleParameterContructor_ParameterIsNull_ThrowException()
        {
            // Arrange
            ChargedCommissionDto dto = null;

            // Act
            TestDelegate creatingObjDelegate = () => new ChargedCommissionItem(dto);

            // Assert
            Assert.Throws(typeof(ArgumentNullException), creatingObjDelegate);
        }

        [Test]
        public void SingleParameterContructor_ParameterIsNotNull_SportTypeIdIsDtoSportTypeId()
        {
            // Arrange
            var dto = new ChargedCommissionDto
            {
                SportTypeId = 1
            };

            // Act
            var chargedComm = new ChargedCommissionItem(dto);

            // Assert
            Assert.AreEqual(1, chargedComm.SportTypeId);
        }

        [Test]
        public void SingleParameterContructor_ParameterIsNotNull_BetTypeIdIsDtoBetTypeId()
        {
            // Arrange
            var dto = new ChargedCommissionDto
            {
                BetTypeId = 1
            };

            // Act
            var chargedComm = new ChargedCommissionItem(dto);

            // Assert
            Assert.AreEqual(1, chargedComm.BetTypeId);
        }

        [Test]
        public void SingleParameterContructor_ParameterIsNotNull_ChargedCommissionIsDtoChargedCommission()
        {
            // Arrange
            var dto = new ChargedCommissionDto
            {
                ChargedCommission = 88
            };

            // Act
            var chargedComm = new ChargedCommissionItem(dto);

            // Assert
            Assert.AreEqual(88, chargedComm.ChargedCommission);
        }

        [Test]
        public void SingleParameterContructor_ParameterIsNotNull_UplineIsNotNull()
        {
            // Arrange
            var dto = new ChargedCommissionDto();

            // Act
            var chargedComm = new ChargedCommissionItem(dto);

            // Assert
            Assert.IsNotNull(chargedComm);
        }

        [Test]
        public void SingleParameterContructor_ParameterIsNotNull_UplineObjChargedCommissionIsDtoUplineChargedCommission()
        {
            // Arrange
            var dto = new ChargedCommissionDto
            {
                UplineChargedCommission = 90
            };

            // Act
            var chargedComm = new ChargedCommissionItem(dto);

            // Assert
            Assert.AreEqual(90, chargedComm.Upline.ChargedCommission);
        }

        [Test]
        public void Validate_Always_ValidationResultIsNotNull()
        {
            // Arrange
            var chargedComm = new ChargedCommissionItem();

            // Act
            var validationResult = chargedComm.Validate();

            // Assert
            Assert.IsNotNull(validationResult);
        }

        [Test]
        public void Validate_UplineIsNull_IsFailed()
        {
            // Arrange
            var chargedComm = new ChargedCommissionItem();

            // Act
            var validationResult = chargedComm.Validate();

            // Assert
            Assert.IsTrue(validationResult.IsFailed);
        }

        [Test]
        public void Validate_UplineIsNull_ReturnInvalidChargedCommissionErrorCode()
        {
            // Arrange
            var chargedComm = new ChargedCommissionItem();

            // Act
            var validationResult = chargedComm.Validate();

            // Assert
            int expectedErrorCode = 10000;
            Assert.AreEqual(expectedErrorCode, validationResult.Code);
        }

        [Test]
        public void Validate_ChargedCommissionIsGreaterThanUplineChargedCommission_ReturnInvalidChargedCommissionErrorCode()
        {
            // Arrange
            var chargedComm = new ChargedCommissionItem
            {
                ChargedCommission = 90,
                Upline = new ChargedCommissionItem()
                {
                    ChargedCommission = 88
                }
            };

            // Act
            var validationResult = chargedComm.Validate();

            // Assert
            int expectedErrorCode = 10000;
            Assert.AreEqual(expectedErrorCode, validationResult.Code);
        }

        [Test]
        public void ToXml_Always_ReturnValidXmlString()
        {
            // Arrange
            var chargedComm = new ChargedCommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                ChargedCommission = 88,
                Upline = new ChargedCommissionItem()
                {
                    ChargedCommission = 90
                }
            };

            // Act
            var actualXml = chargedComm.ToXml();

            // Assert
            var expectedXml = "<ChargedCommission SportType=\"1\" BetType=\"1\" ChargedCommission=\"88\" UplineChargedCommission=\"90\"/>";
            Assert.AreEqual(expectedXml, actualXml);
        }
    }
}