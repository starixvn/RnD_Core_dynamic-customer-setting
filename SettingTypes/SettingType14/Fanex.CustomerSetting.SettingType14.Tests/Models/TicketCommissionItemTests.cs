namespace Fanex.CustomerSetting.SettingType14.Tests.Models
{
    using System;
    using CustomerSetting.Models.Enumerations;
    using Dtos;
    using NUnit.Framework;
    using SettingType14.Models;

    public class TicketCommissionItemTests
    {
        [Test]
        public void ParameterizedContructor_DtoIsNull_ThrowException()
        {
            // Arrange
            TicketCommissionDto dto = null;
            Level customerLevel = Level.Super;
            Level uplineLevel = Level.Company;

            // Act
            TestDelegate creatingObjDelegate = () => new TicketCommissionItem(dto, customerLevel, uplineLevel);

            // Assert
            Assert.Throws(typeof(ArgumentNullException), creatingObjDelegate);
        }

        [Test]
        public void ParameterizedContructor_CustomerLevelIsNull_ThrowException()
        {
            // Arrange
            var dto = new TicketCommissionDto();
            Level customerLevel = null;
            Level uplineLevel = Level.Company;

            // Act
            TestDelegate creatingObjDelegate = () => new TicketCommissionItem(dto, customerLevel, uplineLevel);

            // Assert
            Assert.Throws(typeof(ArgumentNullException), creatingObjDelegate);
        }

        [Test]
        public void ParameterizedContructor_ParameterIsNotNull_SportTypeIdIsDtoSportTypeId()
        {
            // Arrange
            var dto = new TicketCommissionDto
            {
                SportTypeId = 1
            };
            var customerLevel = Level.Super;
            var uplineLevel = Level.Company;

            // Act
            var commission = new TicketCommissionItem(dto, customerLevel, uplineLevel);

            // Assert
            Assert.AreEqual(1, commission.SportTypeId);
        }

        [Test]
        public void SingleParameterContructor_ParameterIsNotNull_BetTypeIdIsDtoBetTypeId()
        {
            // Arrange
            var dto = new TicketCommissionDto
            {
                BetTypeId = 1
            };
            var customerLevel = Level.Super;
            var uplineLevel = Level.Company;

            // Act
            var commission = new TicketCommissionItem(dto, customerLevel, uplineLevel);

            // Assert
            Assert.AreEqual(1, commission.BetTypeId);
        }

        [Test]
        public void SingleParameterContructor_CustomerLevelIsSuper_MapMaxGivenCommissionAndSuperCommissionFromDto()
        {
            // Arrange
            var dto = new TicketCommissionDto
            {
                SportTypeId = 1,
                BetTypeId = 1,
                MaxGivenCommission = 0.1m,
                SuperCommission = 0.09m,
                MasterCommission = 0.08m,
                AgentCommission = 0.07m,
                MemberCommission = 0
            };
            var customerLevel = Level.Super;

            // Act
            var commission = new TicketCommissionItem(dto, customerLevel, null);

            // Assert
            var expectedCommission = new TicketCommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                UplineCommission = 0.1m,
                Commission = 0.09m
            };

            Assert.AreEqual(expectedCommission, commission);
        }

        [Test]
        public void SingleParameterContructor_CustomerLevelIsMaster_MapMaxGivenCommissionAndSuperCommissionFromDto()
        {
            // Arrange
            var dto = new TicketCommissionDto
            {
                SportTypeId = 1,
                BetTypeId = 1,
                MaxGivenCommission = 0.1m,
                SuperCommission = 0.09m,
                MasterCommission = 0.08m,
                AgentCommission = 0.07m,
                MemberCommission = 0
            };
            var customerLevel = Level.Master;

            // Act
            var commission = new TicketCommissionItem(dto, customerLevel, null);

            // Assert
            var expectedCommission = new TicketCommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                UplineCommission = 0.09m,
                Commission = 0.08m
            };

            Assert.AreEqual(expectedCommission, commission);
        }

        [Test]
        public void SingleParameterContructor_CustomerLevelIsAgent_MapMaxGivenCommissionAndSuperCommissionFromDto()
        {
            // Arrange
            var dto = new TicketCommissionDto
            {
                SportTypeId = 1,
                BetTypeId = 1,
                MaxGivenCommission = 0.1m,
                SuperCommission = 0.09m,
                MasterCommission = 0.08m,
                AgentCommission = 0.07m,
                MemberCommission = 0
            };
            var customerLevel = Level.Agent;

            // Act
            var commission = new TicketCommissionItem(dto, customerLevel, null);

            // Assert
            var expectedCommission = new TicketCommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                UplineCommission = 0.08m,
                Commission = 0.07m
            };

            Assert.AreEqual(expectedCommission, commission);
        }

        [Test]
        public void SingleParameterContructor_CustomerLevelIsMemberAndUplineIsSuper_MapMaxGivenCommissionAndSuperCommissionFromDto()
        {
            // Arrange
            var dto = new TicketCommissionDto
            {
                SportTypeId = 1,
                BetTypeId = 1,
                MaxGivenCommission = 0.1m,
                SuperCommission = 0.09m,
                MasterCommission = 0.08m,
                AgentCommission = 0.07m,
                MemberCommission = 0
            };
            var customerLevel = Level.Member;
            var uplineLevel = Level.Super;

            // Act
            var commission = new TicketCommissionItem(dto, customerLevel, uplineLevel);

            // Assert
            var expectedCommission = new TicketCommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                NearestIndirectUplineCommission = 0.08m,
                UplineCommission = 0.07m
            };

            Assert.AreEqual(expectedCommission, commission);
        }

        [Test]
        public void SingleParameterContructor_CustomerLevelIsMemberAndUplineIsMaster_MapMaxGivenCommissionAndSuperCommissionFromDto()
        {
            // Arrange
            var dto = new TicketCommissionDto
            {
                SportTypeId = 1,
                BetTypeId = 1,
                MaxGivenCommission = 0.1m,
                SuperCommission = 0.09m,
                MasterCommission = 0.08m,
                AgentCommission = 0.07m,
                MemberCommission = 0
            };
            var customerLevel = Level.Member;
            var uplineLevel = Level.Master;

            // Act
            var commission = new TicketCommissionItem(dto, customerLevel, uplineLevel);

            // Assert
            var expectedCommission = new TicketCommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                UplineCommission = 0.07m
            };

            Assert.AreEqual(expectedCommission, commission);
        }

        [Test]
        public void SingleParameterContructor_CustomerLevelIsMemberAndUplineIsAgent_MapMaxGivenCommissionAndSuperCommissionFromDto()
        {
            // Arrange
            var dto = new TicketCommissionDto
            {
                SportTypeId = 1,
                BetTypeId = 1,
                MaxGivenCommission = 0.1m,
                SuperCommission = 0.09m,
                MasterCommission = 0.08m,
                AgentCommission = 0.07m,
                MemberCommission = 0
            };
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;

            // Act
            var commission = new TicketCommissionItem(dto, customerLevel, uplineLevel);

            // Assert
            var expectedCommission = new TicketCommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                UplineCommission = 0.07m,
                Commission = 0
            };

            Assert.AreEqual(expectedCommission, commission);
        }

        [Test]
        public void Validate_Always_ValidationResultIsNotNull()
        {
            // Arrange
            var commisison = new TicketCommissionItem();

            // Act
            var validationResult = commisison.Validate();

            // Assert
            Assert.IsNotNull(validationResult);
        }

        [Test]
        public void Validate_CommisisonIsNullAndValidSportTypeBetType_ValidationResultIsSuccess()
        {
            // Arrange
            var commisison = new TicketCommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                Commission = null
            };

            // Act
            var validationResult = commisison.Validate();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }

        [Test]
        public void Validate_CommissionIsLessThanZero_ReturnInvalidCommissionErrorCode()
        {
            // Arrange
            var commisison = new TicketCommissionItem
            {
                Commission = -1m
            };

            // Act
            var validationResult = commisison.Validate();

            // Assert
            int expectedErrorCode = 5102;

            Assert.AreEqual(expectedErrorCode, validationResult.Code);
        }

        [Test]
        public void Validate_CommissionIsGreaterThanOne_ReturnInvalidCommissionErrorCode()
        {
            // Arrange
            var commisison = new TicketCommissionItem
            {
                Commission = 1.5m
            };

            // Act
            var validationResult = commisison.Validate();

            // Assert
            int expectedErrorCode = 5101;

            Assert.AreEqual(expectedErrorCode, validationResult.Code);
        }

        [Test]
        public void ToXml_CommissionIsNotNull_ReturnValidXmlString()
        {
            // Arrange
            var commisison = new TicketCommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                Commission = 0.88m
            };

            // Act
            var actualXml = commisison.ToXml();

            // Assert
            var expectedXml = "<Commission SportType=\"1\" BetType=\"1\" Commission=\"0.88\"/>";

            Assert.AreEqual(expectedXml, actualXml);
        }

        [Test]
        public void ToXml_CommissionIsNull_ReturnEmptyString()
        {
            // Arrange
            var commisison = new TicketCommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1
            };

            // Act
            var actualXml = commisison.ToXml();

            // Assert
            Assert.IsEmpty(actualXml);
        }
    }
}