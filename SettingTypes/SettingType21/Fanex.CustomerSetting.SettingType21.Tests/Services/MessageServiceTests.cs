using System.Collections.Generic;
using System.Linq;
using Fanex.CustomerSetting.Services;
using Fanex.CustomerSetting.SettingType21.Constants;
using Fanex.CustomerSetting.SettingType21.Resources;
using Fanex.CustomerSetting.SettingType21.Services;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.CustomerSetting.SettingType21.Tests.Services
{
    public class MessageServiceTests
    {
        private IMessageService _messageService;
        private int _msgType;
        private string _rawMessage;
        private IResourceService _resourceService;

        [SetUp]
        public void Setup()
        {
            _resourceService = Substitute.For<IResourceService>();
            _messageService = new MessageService(_resourceService);
            _msgType = MessageTypes.ProductStatusViolation;
            _rawMessage = "<Root MsgID=\"60033\" Msg=\"\">"
                          + "<Row SportType=\"1\" BetType=\"1\" />"
                          + "</Root>";
        }

        [TestCase(1, 60033)]
        [TestCase(2, 60033)]
        [TestCase(3, 60033)]
        [TestCase(4, 60033)]
        [TestCase(1, 60035)]
        [TestCase(2, 60035)]
        [TestCase(3, 60035)]
        [TestCase(4, 60035)]
        [TestCase(1, 90012)]
        [TestCase(2, 90012)]
        [TestCase(3, 90012)]
        [TestCase(4, 90012)]
        [TestCase(1, 60037)]
        [TestCase(2, 60037)]
        [TestCase(3, 60037)]
        [TestCase(4, 60037)]
        public void TranslateMessage_Always_MainMessageIsMsgIdFromRawMsg(byte settingLevel, int msgId)
        {
            // Arrange
            _rawMessage = string.Format("<Root MsgID=\"{0}\" Msg=\"\"><Row SportType=\"1\" BetType=\"1\" /></Root>",
                msgId);

            // Act
            var actualMessageContent =
                _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, Arg.Any<string>());

            // Assert
            Assert.AreEqual(Message.ResourceManager.GetString($"Msg_{msgId}"), actualMessageContent.MainMessage);
        }

        [Test]
        public void TranslateMessage_SettingLevelIsProduct_SubMessageIsEmpty()
        {
            // Arrange
            byte settingLevel = 1;

            // Act
            var actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

            // Assert
            Assert.IsEmpty(actualMessageContent.SubMessages.First());
        }

        [Test]
        public void TranslateMessage_SettingLevelIsSportType_SubMessageIsSportTypeName()
        {
            // Arrange
            byte settingLevel = 2;

            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns("Sport type 1");

            // Act
            var actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

            // Assert
            var expectedSubMsg = "Sport type 1";

            Assert.AreEqual(expectedSubMsg, actualMessageContent.SubMessages.First());
        }

        [Test]
        public void TranslateMessage_SettingLevelIsBetType_SubMessageIsBetTypeName()
        {
            // Arrange
            byte settingLevel = 3;

            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns("Bet type 1");

            // Act
            var actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

            // Assert
            var expectedSubMsg = "Bet type 1";

            Assert.AreEqual(expectedSubMsg, actualMessageContent.SubMessages.First());
        }

        [Test]
        public void TranslateMessage_SettingLevelIsSportTypeBetType_SubMessageIsSportTypeNameHyphenBetTypeName()
        {
            // Arrange
            byte settingLevel = 4;

            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns("Sport type 1");
            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns("Bet type 1");

            // Act
            var actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

            // Assert
            var expectedSubMsg = "Sport type 1 - Bet type 1";

            Assert.AreEqual(expectedSubMsg, actualMessageContent.SubMessages.First());
        }
        
        [Test]
        public void TranslateMessage_DependenceViolation()
        {
            // Arrange
            byte settingLevel = 1;

            _resourceService.GetSportTypeName(9904, Arg.Any<string>()).Returns("Jackpot1");
            _resourceService.GetSportTypeName(9905, Arg.Any<string>()).Returns("Jackpot2");
            _resourceService.GetSportTypeName(212, Arg.Any<string>()).Returns("Macau Games_1");
            _resourceService.GetSportTypeName(213, Arg.Any<string>()).Returns("Macau Games_2");
            _msgType = MessageTypes.DependenceViolation;
            _rawMessage = "<Root MsgID=\"60037 \" Msg=\"You cannot Enable {DependedSport} when {ParentSport} is Disabled.\">"
                          + "<Row SportType=\"9904\" BetType=\"4001\" ParentSportType=\"212\" ParentBetType=\"4001\" />"
                          + "<Row SportType=\"9905\" BetType=\"4001\" ParentSportType=\"212\" ParentBetType=\"4001\" />"
                          + "<Row SportType=\"9904\" BetType=\"4001\" ParentSportType=\"213\" ParentBetType=\"4001\" />"
                          + "<Row SportType=\"9905\" BetType=\"4001\" ParentSportType=\"213\" ParentBetType=\"4001\" />"
                          + "</Root>";
            // Act
            var actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

            // Assert
         
            var expectedSubMsg = new List<string>()
            {
                "You cannot Enable Jackpot1, Jackpot2 when Macau Games_1 is Disabled." ,
                "You cannot Enable Jackpot1, Jackpot2 when Macau Games_2 is Disabled."
            };

            Assert.AreEqual(expectedSubMsg, actualMessageContent.SubMessages.ToList());
        }
        
        [Test]
        public void TranslateMessage_Default()
        {
            // Arrange
            byte settingLevel = 1;
             _msgType = 99;
            _rawMessage = "default message";
            // Act
            var actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

            // Assert

            var expectedSubMsg = "default message";

            Assert.AreEqual(expectedSubMsg, actualMessageContent.MainMessage);
        }
    }
}