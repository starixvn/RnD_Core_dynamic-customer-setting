namespace Fanex.CustomerSetting.Client40.Tests.Services
{
    using Fanex.CustomerSetting.Client40.Exceptions;
    using Fanex.CustomerSetting.Client40.Models.Requests;
    using Fanex.CustomerSetting.Client40.Services;
    using NSubstitute;
    using NUnit.Framework;
    using RestSharp;
    using System.Net;
    using System.Web.Script.Serialization;

    [TestFixture]
    class RouteServiceTests
    {
        private IRouteService routeService;
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
        private IRestClient subRestClient;

        [SetUp]
        public void Setup()
        {
            subRestClient = Substitute.For<IRestClient>();
            subRestClient.Execute(Arg.Any<RestRequest>()).Returns(new RestResponse());
            routeService = new RouteService(subRestClient);
        }

        [Test]
        public void Redirect_CommandIsNull_ThrowCustomerSettingException()
        {
            Assert.Throws<CustomerSettingException>(new TestDelegate(
                ()=> routeService.Redirect(null)));
        }

        [Test]
        public void Redirect_CommandIsNotNull_ReturnTimeOut()
        {
            var actualResult = routeService.Redirect(new RedirectRequest());
            var expectedResult = new RestResponse();
            expectedResult.StatusCode = HttpStatusCode.RequestTimeout;
            expectedResult.Content = "{ \"message\": \"Process is timeout.\" }";
            Assert.AreEqual(serializer.Serialize(expectedResult), serializer.Serialize(actualResult));
        }
    }
}
