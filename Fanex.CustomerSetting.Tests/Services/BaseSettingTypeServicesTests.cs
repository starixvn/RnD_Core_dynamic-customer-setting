namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Messages;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingTypeServicesTests : SettingTypeService
    {
        private const string DefaultLanguage = "en-US";

        public SettingTypeServicesTests()
        {
            ResourceService = Substitute.For<IResourceService>();
            Logger = Substitute.For<ILogger>();
            MessageService = Substitute.For<IMessageService>();
        }

        [Test]
        public void SetSportTypeBetTypeName_WhenCalled_SetSportTypeBetTypeNameForSettingItems()
        {
            //// Arrange
            SettingType settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>
                    {
                        new DefaultSettingItem
                        {
                            BetTypeId = 1,
                            SportTypeId = 1
                        }
                    }
            };
            var sportTypeName = "SportTypeName";
            var betTypeName = "BetTypeName";
            ResourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            ResourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualSettingType = this.SetSportTypeBetTypeName(settingType, DefaultLanguage);

            //// Assert
            DefaultSettingItem settingItem = actualSettingType.SettingItems.First() as DefaultSettingItem;
            Assert.AreEqual(sportTypeName, settingItem.SportTypeName);
            Assert.AreEqual(betTypeName, settingItem.BetTypeName);
        }

        [Test]
        public void GetCustomerSettingType_TypeOfSettingItemIsNotDefaultSettingItem_ThrowInvalidCastException()
        {
            //// Arrange
            SettingType settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>
                    {
                        new SettingItemStub()
                    }
            };

            //// Act
            Assert.Throws<InvalidCastException>(
                new TestDelegate(
                    () => this.SetSportTypeBetTypeName(settingType, DefaultLanguage)));
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
                    () => this.TranslateUpdateResultMessage(settingResult, successMessage, settingLevel, DefaultLanguage)));
        }

        [Test]
        public void TranslateUpdateResultMessage_SettingResultIsSuccess_ReturnSuccessMessage()
        {
            //// Arrange
            SettingResult settingResult = new SettingResult(Result.Success);
            string successMessage = "Success Message";
            byte settingLevel = 1;

            //// Act
            var actualSettingResult = this.TranslateUpdateResultMessage(settingResult, successMessage, settingLevel, DefaultLanguage);

            //// Assert
            Assert.AreEqual(successMessage, actualSettingResult.Message);
        }

        [Test]
        public void TranslateUpdateResultMessage_SettingResultIsFailed_AlwaysCallTranslateMessage()
        {
            //// Arrange
            SettingResult settingResult = new SettingResult(code: 1, message: "Failed", msgType: 1, customerId: 1, settingTypeId: 1, settingTypeName: "SettingTypeName");
            string successMessage = "Success Message";
            byte settingLevel = 1;
            var messageContent = new MessageContent("MainMessage");

            MessageService.TranslateMessage(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>()).Returns(messageContent);

            //// Act
            var actualSettingResult = this.TranslateUpdateResultMessage(settingResult, successMessage, settingLevel, DefaultLanguage);

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
            SettingResult settingResult = new SettingResult(code: 1, message: "Failed", msgType: 1, customerId: 1, settingTypeId: 1, settingTypeName: "SettingTypeName");
            string successMessage = "Success Message";
            byte settingLevel = 1;
            var messageContent = new MessageContent("MainMessage");

            MessageService.TranslateMessage(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>()).Returns(messageContent);

            //// Act
            var actualSettingResult = this.TranslateUpdateResultMessage(settingResult, successMessage, settingLevel, DefaultLanguage);

            //// Assert
            Assert.AreEqual(messageContent.MainMessage, actualSettingResult.Message);
        }

        [Test]
        public void UpdateMultipleCustomersSettingType_Always_ReturnListOfUpdateCustomerSettingTypeResult()
        {
            //// Act
            var customerIds = new List<int> { 1, 2 };

            //// Act
            var settingTypeResults = this.UpdateMultipleCustomersSettingType(
                Arg.Any<ISettingContext>(),
                customerIds,
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<byte>(),
                Arg.Any<SettingType>());

            //// Assert
            Assert.AreEqual(customerIds.Count, settingTypeResults.Count());
        }

        ////[Test]
        ////public void UpdateMultipleCustomersSettingType_RaisedException_AlwaysCallLogError()
        ////{
        ////    //// Act
        ////    var customerIds = new List<int> { 0 };
        ////    var settingContext = new SettingContextStub();

        ////    //// Act
        ////    this.UpdateMultipleCustomersSettingType(
        ////        settingContext,
        ////        customerIds,
        ////        Level.Member,
        ////        Level.Agent,
        ////        Arg.Any<byte>(),
        ////        new SettingType());

        ////    //// Assert
        ////    Logger.Received().Error(Arg.Any<string>(), Arg.Any<CustomerSettingException>());
        ////}

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, QuerySettingParam queryParam, SettingType productSettingType)
        {
            throw new NotImplementedException();
        }

        public override SettingResult UpdateCustomerSettingType(ISettingContext settingContext, int customerId, Level customerLevel, Level uplineLevel, byte productId, SettingType settingType)
        {
            if (customerId == 0)
            {
                throw new ArgumentOutOfRangeException("customerId");
            }

            return new SettingResult(Result.Success);
        }
    }
}