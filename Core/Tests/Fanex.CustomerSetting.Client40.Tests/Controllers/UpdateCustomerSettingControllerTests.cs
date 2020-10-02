namespace Fanex.CustomerSetting.Client40.Tests.Controllers
{
    using Fanex.CustomerSetting.Client40.Controllers;
    using Fanex.CustomerSetting.Client40.Models.Requests;
    using Fanex.CustomerSetting.Client40.Services;
    using NSubstitute;
    using NUnit.Framework;
    using RestSharp;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Script.Serialization;

    [TestFixture]
    public class UpdateCustomerSettingControllerTests
    {
        private readonly JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        private UpdateCustomerSettingController controller;

        public UpdateCustomerSettingControllerTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            var subCustomerSettingService = Substitute.For<ICustomerSettingService>();
            var subRestClient = Substitute.For<IRestClient>();
            var subControllerContext = Substitute.For<HttpContextBase>();
            subRestClient.Execute(Arg.Any<IRestRequest>()).Returns(new RestResponse());
            subCustomerSettingService.UpdateCustomerSetting(Arg.Any<UpdateSettingRequest>()).Returns(subRestClient.Execute(null));
            controller = new UpdateCustomerSettingController(subCustomerSettingService);
            controller.ControllerContext = new ControllerContext(subControllerContext, new RouteData(), controller);
        }

        [Test]
        public void UpdateSetting_Always_ReturnExpectedResult()
        {   
            var actualResult = controller.UpdateSetting(new UpdateSettingRequest());
            var expectedResult = new ContentResult();
            expectedResult.Content = string.Empty;

            Assert.AreEqual(javaScriptSerializer.Serialize(expectedResult), javaScriptSerializer.Serialize(actualResult));
        }

        [Test]
        public void UpdateMultipleSetting_Always_ReturnExpectedResult()
        {
            var actualResult = controller.UpdateMultipleSetting(new MultipleUpdateSettingRequest());
            var expectedResult = new ContentResult();
            expectedResult.Content = string.Empty;

            Assert.AreEqual(javaScriptSerializer.Serialize(expectedResult), javaScriptSerializer.Serialize(actualResult));
        }

        [Test]
        public void UpdateCustomerSettingController_Always_NoException()
        {
            Assert.DoesNotThrow(
                new TestDelegate(
                    ()=> new UpdateCustomerSettingController()));
        }
    }
}
