namespace Fanex.CustomerSetting.Tests.Models
{
    using CustomerSetting.Models;
    using CustomerSetting.Models.Constants;
    using CustomerSetting.Models.Results;
    using NUnit.Framework;
    using Resource;

    [TestFixture]
    public class SettingItemErrorTests
    {
        [Test]
        public void ErrorMessage_Always_ReturnSettingTypeErrorMessage()
        {
            var errorItem = new SettingItemError();

            Assert.AreEqual(Message.SettingTypeError, errorItem.ErrorMessage);
        }

        [Test]
        public void TypeName_Always_ReturnExpectedName()
        {
            var errorItem = new SettingItemError();

            Assert.AreEqual(errorItem.GetType().AssemblyQualifiedName, errorItem.TypeName);
        }

        [Test]
        public void Validate_Always_ReturnFailedSettingResult()
        {
            //// Arrange
            var errorItem = new SettingItemError();
            IResult expected = new Result(ResultValue.Error, Message.SettingTypeError);

            //// Act
            IResult settingResult = errorItem.Validate();

            //// Assert
            Assert.AreEqual(expected, settingResult);
        }

        [Test]
        public void ToXml_Always_ReturnExpectedString()
        {
            var errorItem = new SettingItemError();
            var expectedXml = $"<Error>{Message.SettingTypeError}</Error>";

            Assert.AreEqual(expectedXml, errorItem.ToXml());
        }
    }
}