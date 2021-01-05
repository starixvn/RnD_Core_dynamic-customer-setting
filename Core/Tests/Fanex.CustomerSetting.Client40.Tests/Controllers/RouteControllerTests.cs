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
    public class RouteControllerTests
    {
        private readonly JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        private RouteController controller;

        public RouteControllerTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            var subRouteService = Substitute.For<IRouteService>();
            var subRestClient = Substitute.For<IRestClient>();
            var subControllerContext = Substitute.For<HttpContextBase>();
            subRestClient.Execute(Arg.Any<IRestRequest>()).Returns(new RestResponse());
            subRouteService.Redirect(Arg.Any<RedirectRequest>()).Returns(subRestClient.Execute(null));
            controller = new RouteController(subRouteService);
            controller.ControllerContext = new ControllerContext(subControllerContext, new RouteData(), controller);
        }

        [Test]
        public void Redirect_Always_ReturnExpectedResult()
        {
            var actualResult = controller.Redirect(new RedirectRequest());
            var expectedResult = new ContentResult();
            expectedResult.Content = string.Empty;

            Assert.AreEqual(javaScriptSerializer.Serialize(expectedResult), javaScriptSerializer.Serialize(actualResult));
        }

        [Test]
        public void RouteController_Always_NoException()
        {
            Assert.DoesNotThrow(
                new TestDelegate(
                    () => new RouteController()));
        }
    }
}
