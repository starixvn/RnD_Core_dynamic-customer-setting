namespace Fanex.CustomerSetting.SettingType4.Template1.Tests.Services
{
    using System.Linq;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Resources;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.SettingType4.Template1.Resources;
    using Fanex.CustomerSetting.SettingType4.Template1.Services;
    using Fanex.CustomerSetting.Tests.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ProductStatusMessageServiceTests
    {
        private ISettingContext _settingContext;
        private IResourceService _resourceService;
        private IMessageService _productStatusMessageService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _resourceService = Substitute.For<IResourceService>();
            _productStatusMessageService = new ProductStatusMessageService(_resourceService);
        }

        #region Simple Message

        [Test]
        public void TranslateMessage_MessageTypeIsException_ReturnMatchedMessageContent()
        {
            //// Arrange
            int msgType = 100; ////Exception
            string rawMessage = "<Root MsgID=\"90001\" Msg=\"Update process is unsuccessful!\"/>";
            byte settingLevel = 1;

            //// Act
            var actualMessageContent = _productStatusMessageService.TranslateMessage(msgType, rawMessage, settingLevel, _settingContext.Language);

            //// Assert
            Assert.AreEqual(Error.Msg_90001, actualMessageContent.MainMessage);
        }

        [Test]
        public void TranslateMessage_MessageIdIsNotFound_ReturnUndefinedMessage()
        {
            //// Arrange
            int msgType = 100; ////Exception
            string rawMessage = "<Root MsgID=\"121212\"/>";
            byte settingLevel = 1;

            //// Act
            var actualMessageContent = _productStatusMessageService.TranslateMessage(msgType, rawMessage, settingLevel, _settingContext.Language);

            //// Assert
            Assert.AreEqual("Undefined error message 121212", actualMessageContent.MainMessage);
        }

        #endregion Simple Message

        #region Other Message

        [Test]
        public void TranslateMessage_MessageTypeIsOther_ReturnRawMessage()
        {
            //// Arrange
            int msgType = 101; ////Exception
            string rawMessage = "Raw Message";
            byte settingLevel = 1;

            //// Act
            var actualMessageContent = _productStatusMessageService.TranslateMessage(msgType, rawMessage, settingLevel, _settingContext.Language);

            //// Assert
            Assert.AreEqual(rawMessage, actualMessageContent.MainMessage);
        }

        #endregion Other Message

        [TestCase(1, "", "", "")]
        [TestCase(2, "SportType1", "", "SportType1")]
        [TestCase(3, "", "BetType1", "BetType1")]
        [TestCase(4, "SportType1", "BetType1", "SportType1 - BetType1")]
        public void TranslateMessage_MessageTypeIsWrongProductStatus_ReturnMatchedMessageContent(
            byte settingLevel,
            string sportTypeName,
            string betTypeName,
            string expectedSubMessage)
        {
            /// Arrange
            int msgType = 91; //// Wrong Product Status
            var rawMessage = "<Root MsgID=\"60031\" Msg=\"\">"
                                + "<Row SportType=\"1\" BetType=\"1\" />"
                                + "</Root>";
            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = _productStatusMessageService.TranslateMessage(msgType, rawMessage, settingLevel, _settingContext.Language);

            //// Assert
            Assert.AreEqual(ProductStatusError.Msg_60031, actualMessageContent.MainMessage);
            Assert.AreEqual(expectedSubMessage, actualMessageContent.SubMessages.First());
        }
    }
}