namespace Fanex.CustomerSetting.Tests.Services
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using NUnit.Framework;

    [TestFixture]
    public abstract class BaseMessageServiceTests
    {
        private const int ExceptionMessageType = 100;
        private const int OtherMessageType = 101;

        protected ISettingContext SettingContext { get; set; }

        protected IResourceService ResourceService { get; set; }

        protected IMessageService MessageService { get; set; }

        [Test]
        public virtual void TranslateMessage_MessageTypeIsException_ReturnMatchedMessageContent()
        {
            //// Arrange
            int msgType = ExceptionMessageType;
            string rawMessage = "<Root MsgID=\"90001\" Msg=\"Update process is unsuccessful!\"/>";
            byte settingLevel = 1;

            //// Act
            var actualMessageContent = MessageService.TranslateMessage(msgType, rawMessage, settingLevel, "en-US");

            //// Assert
            Assert.AreEqual(Message.Msg_90001, actualMessageContent.MainMessage);
        }

        [Test]
        public virtual void TranslateMessage_MessageIdIsNotFound_ReturnUndefinedMessage()
        {
            //// Arrange
            int msgType = ExceptionMessageType;
            string rawMessage = "<Root MsgID=\"121212\"/>";
            byte settingLevel = 1;

            //// Act
            var actualMessageContent = MessageService.TranslateMessage(msgType, rawMessage, settingLevel, "en-US");

            //// Assert
            Assert.AreEqual("Undefined error message 121212", actualMessageContent.MainMessage);
        }

        [Test]
        public virtual void TranslateMessage_MessageTypeIsOther_ReturnRawMessage()
        {
            //// Arrange
            int msgType = OtherMessageType;
            string rawMessage = "Raw Message";
            byte settingLevel = 1;

            //// Act
            var actualMessageContent = MessageService.TranslateMessage(msgType, rawMessage, settingLevel, "en-US");

            //// Assert
            Assert.AreEqual(rawMessage, actualMessageContent.MainMessage);
        }
    }
}