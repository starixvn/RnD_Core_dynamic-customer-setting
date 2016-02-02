namespace Fanex.CustomerSetting.SettingType7.Tests.Services
{
    using System.Linq;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType7.Constants;
    using Fanex.CustomerSetting.SettingType7.Services;
    using NSubstitute;
    using NUnit.Framework;

    public class MessageServiceTests
    {
        private IResourceService _resourceService;
        private IMessageService _messageService;
        private int _msgType;
        private string _rawMessage;

        [SetUp]
        public void Setup()
        {
            _resourceService = Substitute.For<IResourceService>();
            _messageService = new MessageService(_resourceService);
            _msgType = MessageType.StakeLevelViolation;
            _rawMessage = "<Root MsgID=\"1\" Msg=\"\">"
                            + "<Row SportType=\"1\" BetType=\"1\" />"
                        + "</Root>";
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
            string expectedSubMsg = "Sport type 1";
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
            string expectedSubMsg = "Bet type 1";
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
            string expectedSubMsg = "Sport type 1 - Bet type 1";
            Assert.AreEqual(expectedSubMsg, actualMessageContent.SubMessages.First());
        }
    }
}