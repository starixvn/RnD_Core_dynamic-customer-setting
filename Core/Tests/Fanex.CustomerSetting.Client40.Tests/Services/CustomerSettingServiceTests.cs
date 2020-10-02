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
    public class CustomerSettingServiceTests
    {
        private ICustomerSettingService customerSettingService;
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
        private IRestClient subRestClient;


        [SetUp]
        public void Setup()
        {
            subRestClient = Substitute.For<IRestClient>();
            subRestClient.Execute(Arg.Any<RestRequest>()).Returns(new RestResponse());
            customerSettingService = new CustomerSettingService(subRestClient);
        }

        [Test]
        public void QueryCustomerSetting_QueryIsNull_ThrowCustomerSettingException()
        {
            Assert.Throws<CustomerSettingException>(new TestDelegate(
                () => customerSettingService.QueryCustomerSetting(null)));
        }

        [Test]
        public void QueryCustomerSetting_QueryIsNotNull_ReturnRestResponse()
        {
            var actualResult = customerSettingService.QueryCustomerSetting(new QuerySettingRequest());
            var expectedResult = new RestResponse();
            Assert.AreEqual(serializer.Serialize(expectedResult), serializer.Serialize(actualResult));
        }

        [Test]
        public void UpdateCustomerSetting_QueryIsNull_ThrowCustomerSettingException()
        {
            Assert.Throws<CustomerSettingException>(new TestDelegate(
                () => customerSettingService.UpdateCustomerSetting(null)));
        }

        [Test]
        public void UpdateCustomerSetting_QueryIsNotNull_ReturnTimeOut()
        {
            var actualResult = customerSettingService.UpdateCustomerSetting(new UpdateSettingRequest());
            var expectedResult = new RestResponse();
            expectedResult.StatusCode = HttpStatusCode.RequestTimeout;
            expectedResult.Content = "{ \"message\": \"Update Process is timeout.\" }";
            Assert.AreEqual(serializer.Serialize(expectedResult), serializer.Serialize(actualResult));
        }

        [Test]
        public void UpdateCustomerSetting_QueryIsNotNull_ReturnRestResponseWithStatusCodeOK()
        {
            var successRestResponse = new RestResponse();
            successRestResponse.StatusCode = HttpStatusCode.OK;
            subRestClient.Execute(Arg.Any<RestRequest>()).Returns(successRestResponse);
            var actualResult = customerSettingService.UpdateCustomerSetting(new UpdateSettingRequest());
            var expectedResult = new RestResponse();
            expectedResult.StatusCode = HttpStatusCode.OK;
            Assert.AreEqual(serializer.Serialize(expectedResult), serializer.Serialize(actualResult));
        }

        [Test]
        public void MultipleUpdateCustomerSetting_QueryIsNull_ThrowCustomerSettingException()
        {
            Assert.Throws<CustomerSettingException>(new TestDelegate(
                () => customerSettingService.MultipleUpdateCustomerSetting(null)));
        }

        [Test]
        public void MultipleUpdateCustomerSetting_QueryIsNotNull_ReturnTimeOut()
        {
            var actualResult = customerSettingService.MultipleUpdateCustomerSetting(new MultipleUpdateSettingRequest());
            var expectedResult = new RestResponse();
            expectedResult.StatusCode = HttpStatusCode.RequestTimeout;
            expectedResult.Content = "{ \"message\": \"Update Process is timeout.\" }";
            Assert.AreEqual(serializer.Serialize(expectedResult), serializer.Serialize(actualResult));
        }

        [Test]
        public void MultipleUpdateCustomerSetting_QueryIsNotNull_ReturnRestResponseWithStatusCodeOK()
        {
            var successRestResponse = new RestResponse();
            successRestResponse.StatusCode = HttpStatusCode.OK;
            subRestClient.Execute(Arg.Any<RestRequest>()).Returns(successRestResponse);
            var actualResult = customerSettingService.MultipleUpdateCustomerSetting(new MultipleUpdateSettingRequest());
            var expectedResult = new RestResponse();
            expectedResult.StatusCode = HttpStatusCode.OK;
            Assert.AreEqual(serializer.Serialize(expectedResult), serializer.Serialize(actualResult));
        }

        [Test]
        public void QueryCustomerSettingTemplate_Always_ReturnRestResponse()
        {
            var actualResult = customerSettingService.QueryCustomerSettingTemplate();
            Assert.IsTrue(actualResult.GetType() == typeof(RestResponse));
        }
    }
}
