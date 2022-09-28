namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Messages;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using CustomerSetting.LoggingConfig;
    using Fanex.Logging.Extensions.Database;
    using Logging;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingTypeServiceBaseTests : SettingTypeServiceBase
    {
        private const string DefaultLanguage = "en-US";
        static SettingTypeServiceBaseTests()
        {
            LoggingConfig.Init(new[] { "pass", "pwd", "security", "Password" }, getDbExceptionInfo: DatabaseInfoGetters.GetDbExceptionInfo);
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            ResourceService = Substitute.For<IResourceService>();
            Logger = Substitute.For<ILogger>();
            MessageService = Substitute.For<IMessageService>();
        }

        [Test]
        public void SetSportTypeBetTypeName_WhenCalled_SetSportTypeBetTypeNameForSettingItems()
        {
            //// Arrange
            var settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>
                    {
                        new SettingItemDefault
                        {
                            BetTypeId = 1,
                            SportTypeId = 1
                        },
                        new SettingItemDefault()
                    }
            };
            var sportTypeName = "SportTypeName";
            var betTypeName = "BetTypeName";
            ResourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            ResourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualSettingType = SetSportTypeBetTypeName(settingType, DefaultLanguage);

            //// Assert
            var settingItem = actualSettingType.SettingItems.First() as SettingItemDefault;
            Assert.AreEqual(sportTypeName, settingItem.SportTypeName);
            Assert.AreEqual(betTypeName, settingItem.BetTypeName);
        }

        [Test]
        public void GetCustomerSettingType_TypeOfSettingItemIsNotDefaultSettingItem_ThrowInvalidCastException()
        {
            //// Arrange
            var settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>
                    {
                        new SettingItemStub()
                    }
            };

            //// Act
            Assert.Throws<InvalidCastException>(
                new TestDelegate(
                    () => SetSportTypeBetTypeName(settingType, DefaultLanguage)));
        }

        [Test]
        public void TranslateUpdateResultMessage_SettingResultIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            SettingResult settingResult = null;
            string successMessage = "Success Message";
            byte settingLevel = 1;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => TranslateUpdateResultMessage(settingResult, successMessage, settingLevel, DefaultLanguage)));
        }

        [Test]
        public void TranslateUpdateResultMessage_SettingResultIsSuccess_ReturnSuccessMessage()
        {
            //// Arrange
            var settingResult = new SettingResult(Result.Success);
            string successMessage = "Success Message";
            byte settingLevel = 1;

            //// Act
            var actualSettingResult = TranslateUpdateResultMessage(settingResult, successMessage, settingLevel, DefaultLanguage);

            //// Assert
            Assert.AreEqual(successMessage, actualSettingResult.Message);
        }

        [Test]
        public void TranslateUpdateResultMessage_SettingResultIsFailed_AlwaysCallTranslateMessage()
        {
            //// Arrange
            var settingResult = new SettingResult(code: 1, message: "Failed", msgType: 1, customerId: 1, settingTypeId: 1, settingTypeName: "SettingTypeName");
            string successMessage = "Success Message";
            byte settingLevel = 1;
            var messageContent = new MessageContent("MainMessage");

            MessageService.TranslateMessage(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>()).Returns(messageContent);

            //// Act
            TranslateUpdateResultMessage(settingResult, successMessage, settingLevel, DefaultLanguage);

            //// Assert
            MessageService.Received().TranslateMessage(
                Arg.Is(settingResult.MsgType),
                Arg.Is(settingResult.Message),
                Arg.Is(settingLevel),
                Arg.Is(DefaultLanguage));
        }

        [Test]
        public void TranslateUpdateResultMessage_SettingResultIsFailed_AlwaysReturnTranslatedMessage()
        {
            //// Arrange
            var settingResult = new SettingResult(code: 1, message: "Failed", msgType: 1, customerId: 1, settingTypeId: 1, settingTypeName: "SettingTypeName");
            string successMessage = "Success Message";
            byte settingLevel = 1;
            var messageContent = new MessageContent("MainMessage");

            MessageService.TranslateMessage(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>()).Returns(messageContent);

            //// Act
            var actualSettingResult = TranslateUpdateResultMessage(settingResult, successMessage, settingLevel, DefaultLanguage);

            //// Assert
            Assert.AreEqual(messageContent.MainMessage, actualSettingResult.Message);
        }

        [Test]
        public void UpdateMultipleCustomersSettingType_CustomerIdsIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UpdateMultipleCustomerSettingType(
                Arg.Any<ISettingContext>(),
                null,
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<byte>(),
                Arg.Any<SettingType>()));

        }

        [Test]
        public void UpdateMultipleCustomersSettingType_Always_ReturnListOfUpdateCustomerSettingTypeResult()
        {
            //// Act
            var customerIds = new List<int> { 1, 2 };

            //// Act
            var settingTypeResults = UpdateMultipleCustomerSettingType(
                Arg.Any<ISettingContext>(),
                customerIds,
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<byte>(),
                Arg.Any<SettingType>());

            //// Assert
            Assert.AreEqual(customerIds.Count, settingTypeResults.Count());
        }

        [Test]
        public void UpdateMultipleCustomersSettingType_RaisedException_ReturnErrorResult()
        {
            //// Act
            var customerIds = new List<int> { 0 };
            var settingContext = new SettingContextStub();

            //// Act
            var updateResults = UpdateMultipleCustomerSettingType(
                settingContext,
                customerIds,
                Level.Member,
                Level.Agent,
                Arg.Any<byte>(),
                new SettingType());

            //// Assert
            Assert.IsFalse(updateResults.First().IsSuccessful);
        }

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, CustomerSettingQuery query)
        {
            throw new NotImplementedException();
        }

        public override SettingResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType)
        {
            if (customerId == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(customerId));
            }

            return new SettingResult(Result.Success);
        }
    }
}