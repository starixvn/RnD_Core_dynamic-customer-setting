namespace Fanex.CustomerSetting.SettingType17.Tests.Services
{
    using System.Linq;
    using Constants;
    using CustomerSetting.Services;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using SettingType17.Services;

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
            _msgType = MessageTypes.StakeLevelViolation;
            _rawMessage = "<Root MsgID=\"10035\" Msg=\"\">"
                            + "<Row SportType=\"1\" BetType=\"1\" />"
                        + "</Root>";
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TranslateMessage_Always_MainMessageIsMsg1(byte settingLevel)
        {
            // Act
            var actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, Arg.Any<string>());

            // Assert
            Assert.AreEqual(Message.Msg_10035, actualMessageContent.MainMessage);
        }
    }
}