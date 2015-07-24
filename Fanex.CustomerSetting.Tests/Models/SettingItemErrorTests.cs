namespace Fanex.CustomerSetting.Tests.Models
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Results;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class SettingItemErrorTests
    {
        [Test]
        public void ErrorMessage_Always_ReturnSettingTypeErrorMessage()
        {
            var errorItem = new SettingItemError();

            errorItem.ErrorMessage.ShouldBeEquivalentTo<string>(Message.SettingTypeError);
        }

        [Test]
        public void TypeName_Always_ReturnExpectedName()
        {
            var errorItem = new SettingItemError();

            errorItem.TypeName.ShouldBeEquivalentTo<string>(errorItem.GetType().AssemblyQualifiedName);
        }

        [Test]
        public void Validate_Always_ReturnFailedSettingResult()
        {
            //// Arrange
            var errorItem = new SettingItemError();
            IResult expected = new Result(Constants.ResultValue.Error, Message.SettingTypeError);

            //// Act
            IResult settingResult = errorItem.Validate();
            
            //// Assert
            settingResult.ShouldBeEquivalentTo<IResult>(expected);
        }

        [Test]
        public void ToXml_Always_ReturnExpectedString()
        {
            var errorItem = new SettingItemError();

            errorItem.ToXml().ShouldBeEquivalentTo<string>(string.Format("<Error>{0}</Error>", Message.SettingTypeError));
        }
    }
}
