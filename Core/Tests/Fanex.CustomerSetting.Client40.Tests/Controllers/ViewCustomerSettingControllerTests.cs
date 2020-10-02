namespace Fanex.CustomerSetting.Client40.Tests.Controllers
{
    using Fanex.CustomerSetting.Client40.Attributes;
    using Fanex.CustomerSetting.Client40.Controllers;
    using Fanex.CustomerSetting.Client40.Exceptions;
    using Fanex.CustomerSetting.Client40.Models;
    using Fanex.CustomerSetting.Client40.Models.Requests;
    using Fanex.CustomerSetting.Client40.Services;
    using NSubstitute;
    using NUnit.Framework;
    using RestSharp;
    using System;
    using System.IO;
    using System.Runtime.Caching;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Script.Serialization;


    class ViewCustomerSettingControllerTests
    {

        private readonly JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        private ViewCustomerSettingController controller;
        private RestResponse restResponse;


        [SetUp]
        public void Setup()
        {
            var subCustomerSettingService = Substitute.For<ICustomerSettingService>();
            var subRestClient = Substitute.For<IRestClient>();
            var subControllerContext = Substitute.For<HttpContextBase>();
            var successRestResponse = new RestResponse();
            successRestResponse.StatusCode = System.Net.HttpStatusCode.OK;
            successRestResponse.Content = "\"\"";
            restResponse = successRestResponse;
            subRestClient.Execute(null).Returns(restResponse);
            subCustomerSettingService.QueryCustomerSetting(Arg.Any<QuerySettingRequest>()).Returns(restResponse);
            subCustomerSettingService.QueryCustomerSettingTemplate().Returns(restResponse);
            controller = new ViewCustomerSettingController(subCustomerSettingService);
            controller.ControllerContext = new ControllerContext(subControllerContext, new RouteData(), controller);
            controller.Url = new UrlHelper(controller.ControllerContext.RequestContext);
            ObjectCache cache = MemoryCache.Default;
            cache.Set("CustomerSettingTemplate", string.Empty, DateTime.Now.AddSeconds(1));
        }

        [Test]
        public void Constructor_Parameterless_NotThrowException()
        {
            Assert.DoesNotThrow(new TestDelegate(
                () => new ViewCustomerSettingController()));
        }

        [Test]
        public void Index_TemplateIsEmpty_ReturnExpectedResult()
        {
            var actualResult = controller.Index(new SettingFormQuery());
            var expectedResult = new ContentResult();
            expectedResult.Content = string.Empty;
            expectedResult.ContentType = "text/HTML";
            Assert.AreEqual(javaScriptSerializer.Serialize(expectedResult), javaScriptSerializer.Serialize(actualResult));
        }

        [Test]
        public void Index_TemplateIsNotEmpty_NoException()
        {
            AntiForgeryExtension.GetTokensMethod = FakeGetTokens;
            string fakeTemplate = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\Templates\\customer-setting.htm");
            ObjectCache cache = MemoryCache.Default;
            cache.Set("CustomerSettingTemplate", fakeTemplate, DateTime.Now.AddSeconds(1));
            Assert.DoesNotThrow(new TestDelegate(
                ()=> controller.Index(new SettingFormQuery())));
        }

        private static void FakeGetTokens(string oldCookieToken, out string newCookieToken, out string formToken)
        {
            newCookieToken = string.Empty;
            formToken = string.Empty;
        }

        [Test]
        public void Index_QueryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), new TestDelegate(
                () => controller.Index(null)));
        }

        [Test]
        public void Index_NetWorkError_ThrowCustomerSettingException()
        {
            restResponse.StatusCode = System.Net.HttpStatusCode.NotFound;

            Assert.Throws(typeof(CustomerSettingException), new TestDelegate(
                () => controller.Index(new SettingFormQuery())));
        }

        [Test]
        public void Index_FlexboxInapplicableIsTrue_ReturnTemplateWithoutFlexboxCss()
        {
            controller.FlexboxInapplicable = true;

            AntiForgeryExtension.GetTokensMethod = FakeGetTokens;
            string fakeTemplate = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\Templates\\customer-setting.htm");
            ObjectCache cache = MemoryCache.Default;
            cache.Set("CustomerSettingTemplate", fakeTemplate, DateTime.Now.AddSeconds(1));
            var actualResult = controller.Index(new SettingFormQuery());

            Assert.IsTrue(actualResult.Content.Contains("{{flexboxPatch}}"));
        }

        [Test]
        public void QuerySetting_Null_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(
                ()=>controller.QuerySetting(null)));
        }

        [Test]
        public void QuerySetting_ValidRequest_ReturnExpectedResult()
        {
            var actualResult = controller.QuerySetting(new QuerySettingRequest());
            var expectedResult = new ContentResult();
            expectedResult.Content = "\"\"";

            Assert.AreEqual(javaScriptSerializer.Serialize(expectedResult), javaScriptSerializer.Serialize(actualResult));
        }
    }
}
