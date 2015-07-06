namespace Fanex.CustomerSetting.SettingType4.Template1.Tests.Services
{
    using System.Linq;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType4.Template1.Resources;
    using Fanex.CustomerSetting.SettingType4.Template1.Services;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.CustomerSetting.Tests.Services;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ProductStatusMessageServiceTests : BaseMessageServiceTests
    {
        [SetUp]
        public void Setup()
        {
            SettingContext = new SettingContextStub();
            ResourceService = Substitute.For<IResourceService>();
            MessageService = new ProductStatusMessageService(ResourceService);
        }

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
            ResourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            ResourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = MessageService.TranslateMessage(msgType, rawMessage, settingLevel, SettingContext.Language);

            //// Assert
            Assert.AreEqual(ProductStatusMessage.Msg_60031, actualMessageContent.MainMessage);
            Assert.AreEqual(expectedSubMessage, actualMessageContent.SubMessages.First());
        }
    }
}