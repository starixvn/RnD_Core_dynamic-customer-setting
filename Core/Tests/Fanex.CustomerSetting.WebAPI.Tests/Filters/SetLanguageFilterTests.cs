namespace Fanex.CustomerSetting.WebAPI.Tests.Filters
{
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http.Controllers;
    using NSubstitute;
    using NUnit.Framework;
    using WebApi.Filters;
    using WebApi.Models.Request;

    [TestFixture]
    public class SetLanguageFilterTests
    {
        private const string ActionArgumentName = "request";
        private const string DefaultLanguage = "en-US";

        private HttpRequestMessage _request;
        private HttpActionContext _actionContext;
        private HttpControllerContext _controllerContext;
        private HttpActionDescriptor _actionDescriptor;
        private SettingRequest _settingRequest;
        private SetLanguageFilterAttribute _filter;

        [SetUp]
        public void SetUp()
        {
            _request = Substitute.ForPartsOf<HttpRequestMessage>();
            _controllerContext = Substitute.ForPartsOf<HttpControllerContext>();
            _actionDescriptor = Substitute.ForPartsOf<HttpActionDescriptor>();
            _settingRequest = Substitute.ForPartsOf<SettingRequest>();
            _actionContext = new HttpActionContext(_controllerContext, _actionDescriptor);
            _actionContext.ControllerContext.Request = _request;
            _actionContext.ActionArguments[ActionArgumentName] = _settingRequest;
            _filter = new SetLanguageFilterAttribute();
        }

        [Test]
        public void OnActionExecuting_ActionContextIsNull_CurrentLanguageIsDefaultLanguage()
        {
            // Arrange.
            _actionContext = null;

            // Act.
            _filter.OnActionExecuting(_actionContext);

            // Assert.
            Assert.AreEqual(DefaultLanguage, Thread.CurrentThread.CurrentCulture.Name);
        }

        [Test]
        public void OnActionExecuting_ActionArgumentsDoesNotContainSettingRequest_CurrentLanguageIsDefaultLanguage()
        {
            // Arrange.
            _actionContext.ActionArguments.Remove(ActionArgumentName);

            // Act.
            _filter.OnActionExecuting(_actionContext);

            // Assert.
            Assert.AreEqual(DefaultLanguage, Thread.CurrentThread.CurrentCulture.Name);
        }

        [Test]
        public void OnActionExecuting_SettingRequestIsNull_CurrentLanguageIsDefaultLanguage()
        {
            // Arrange.
            _actionContext.ActionArguments[ActionArgumentName] = null;

            // Act.
            _filter.OnActionExecuting(_actionContext);

            // Assert.
            Assert.AreEqual(DefaultLanguage, Thread.CurrentThread.CurrentCulture.Name);
        }

        [Test]
        public void OnActionExecuting_RequestMethodIsNotGetOrPost_CurrentLanguageIsDefaultLanguage()
        {
            // Arrange.
            _actionContext.Request.Method = HttpMethod.Put;

            // Act.
            _filter.OnActionExecuting(_actionContext);

            // Assert.
            Assert.AreEqual(DefaultLanguage, Thread.CurrentThread.CurrentCulture.Name);
        }

        [Test]
        public void OnActionExecuting_SettingRequestContainsNewLanguage_CurrentLanguageIsNewLanguage()
        {
            // Arrange.
            _settingRequest.Language = "vi-VN";

            // Act.
            _filter.OnActionExecuting(_actionContext);

            // Assert.
            Assert.AreEqual("vi-VN", Thread.CurrentThread.CurrentCulture.Name);
        }
    }
}