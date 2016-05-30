namespace Fanex.CustomerSetting.Matrix.WebApi.Tests.Extensions
{
    using System.Net.Http;
    using System.Web;
    using NSubstitute;
    using NUnit.Framework;
    using WebApi.Extensions;

    [TestFixture]
    public class HttpRequestMessageExtensionsTests
    {
        private const string HttpContextPropertyKey = "MS_HttpContext";

        private HttpContextBase _context;
        private HttpRequestBase _requestBase;
        private HttpRequestMessage _request;

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<HttpContextBase>();
            _request = Substitute.For<HttpRequestMessage>();
            _requestBase = Substitute.For<HttpRequestBase>();
            _context.Request.Returns(_requestBase);
            _request.Properties[HttpContextPropertyKey] = _context;
        }

        [Test]
        public void GetClientIPAddress_RequestPropertiesDoesNotContainHttpContext_RerturnUnknown()
        {
            // Arrange.
            _request.Properties.Remove(HttpContextPropertyKey);

            // Act.
            string clientIPAddress = _request.GetClientIPAddress();

            // Assert.
            Assert.AreEqual("Unknown", clientIPAddress);
        }

        [Test]
        public void GetClientIPAddress_RequestPropertiesContainsHttpContext_RerturnClientIP()
        {
            // Arrange.
            _requestBase.UserHostAddress.Returns("127.0.0.1");

            // Act.
            string clientIPAddress = _request.GetClientIPAddress();

            // Assert.
            Assert.AreEqual("127.0.0.1", clientIPAddress);
        }

        [Test]
        public void GetBodyForm_Always_ReturnRequestContent()
        {
            // Arrange.
            _request.Content = new StringContent("Simple content");

            // Act.
            string bodyForm = _request.GetBodyForm();

            // Assert.
            Assert.AreEqual("Simple content", bodyForm);
        }
    }
}