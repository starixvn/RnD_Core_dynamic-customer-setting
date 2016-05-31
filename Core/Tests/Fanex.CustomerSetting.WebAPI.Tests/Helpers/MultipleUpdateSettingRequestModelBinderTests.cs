namespace Fanex.CustomerSetting.WebAPI.Tests.Helpers
{
    using System.Net.Http;
    using NUnit.Framework;
    using WebApi.Models.Request;

    [TestFixture]
    public class MultipleUpdateSettingRequestModelBinderTests : SettingRequestModelBinderTests<MultipleUpdateSettingRequest>
    {
        [Test]
        public void BindModel_ValidRequestParam_ModelStateIsValid()
        {
            // Arrange.
            JsonModel = BuildJsonModel();
            Request.Content = new StringContent(JsonModel);

            // Act.
            ModelBinder.BindModel(ActionContext, BindingContext);

            // Assert.
            Assert.IsTrue(ActionContext.ModelState.IsValid);
        }

        [Test]
        public void BindModel_ValidRequestParam_ModelIsNotNull()
        {
            // Arrange.
            JsonModel = BuildJsonModel();
            Request.Content = new StringContent(JsonModel);

            // Act.
            ModelBinder.BindModel(ActionContext, BindingContext);

            // Assert.
            Assert.NotNull(BindingContext.Model);
        }

        [Test]
        public void BindModel_ValidRequestParam_ModelIsTypeOfUpdateSettingRequest()
        {
            // Arrange.
            JsonModel = BuildJsonModel();
            Request.Content = new StringContent(JsonModel);

            // Act.
            ModelBinder.BindModel(ActionContext, BindingContext);

            // Assert.
            Assert.IsInstanceOf<MultipleUpdateSettingRequest>(BindingContext.Model);
        }
    }
}