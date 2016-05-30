namespace Fanex.CustomerSetting.Matrix.WebApi.Tests.Filters
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Http.Controllers;
    using Configurations;
    using NSubstitute;
    using NUnit.Framework;
    using WebApi.Filters;

    [TestFixture]
    public class HmacAuthorizationFilterTests
    {
        private HttpContextBase _context;
        private HttpRequestMessage _request;
        private HttpActionContext _actionContext;
        private HttpControllerContext _controllerContext;
        private HttpActionDescriptor _actionDescriptor;
        private HmacAuthorizationFilter _filter;
        private string _secretKey;

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.ForPartsOf<HttpContextBase>();
            _request = Substitute.ForPartsOf<HttpRequestMessage>();
            _controllerContext = Substitute.ForPartsOf<HttpControllerContext>();
            _actionDescriptor = Substitute.ForPartsOf<HttpActionDescriptor>();
            _actionContext = new HttpActionContext(_controllerContext, _actionDescriptor);
            _actionContext.ControllerContext.Request = _request;
            _request.RequestUri = new Uri("http://localhost:7878/customersetting/getcustomersetting");
            _filter = new HmacAuthorizationFilter();
            _secretKey = SettingMatrixAPIConfiguration.Settings.SecretKey;
        }

        [Test]
        public void OnAuthorization_ActionContextIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            _actionContext = null;

            // Act and Assert.
            var ex = Assert.Throws<ArgumentNullException>(() => _filter.OnAuthorization(_actionContext));

            Assert.AreEqual("actionContext", ex.ParamName);
        }

        [Test]
        public void OnAuthorization_WrongRequestHeader_AccessDenied()
        {
            // Arrange.
            string hmacString = ComputeHmacString(_secretKey);
            string authHeader = CombineAuthenticationHeader(hmacString);

            _request.Headers.Add("WrongRequestHeader", authHeader);

            // Act.
            _filter.OnAuthorization(_actionContext);

            // Assert.
            Assert.AreEqual(HttpStatusCode.Unauthorized, _actionContext.Response.StatusCode);
        }

        [Test]
        public void OnAuthorization_WrongSecretKey_AccessDenied()
        {
            // Arrange.
            string hmacString = ComputeHmacString("U6BUFHtVcq5SPQcTl7tHLBG/LF6d5YVRKHrCHDvvvvw=");
            string authHeader = CombineAuthenticationHeader(hmacString);

            _request.Headers.Add("Authorization", authHeader);

            // Act.
            _filter.OnAuthorization(_actionContext);

            // Assert.
            Assert.AreEqual(HttpStatusCode.Unauthorized, _actionContext.Response.StatusCode);
        }

        [Test]
        public void OnAuthorization_WrongHeaderScheme_AccessDenied()
        {
            // Arrange.
            string hmacString = ComputeHmacString(_secretKey);
            string authHeader = CombineAuthenticationHeader(hmacString, "wrongscheme");

            _request.Headers.Add("Authorization", authHeader);

            // Act.
            _filter.OnAuthorization(_actionContext);

            // Assert.
            Assert.AreEqual(HttpStatusCode.Unauthorized, _actionContext.Response.StatusCode);
        }

        [Test]
        public void OnAuthorization_WrongNumerOfParams_AccessDenied()
        {
            // Arrange.
            string authHeader = $"amx 127.0.0.1:abc:123";

            _request.Headers.Add("Authorization", authHeader);

            // Act.
            _filter.OnAuthorization(_actionContext);

            // Assert.
            Assert.AreEqual(HttpStatusCode.Unauthorized, _actionContext.Response.StatusCode);
        }

        [Test]
        public void OnAuthorization_EncryptedClientIPDiffersFromRequestIP_AccessDenied()
        {
            // Arrange.
            DateTime requestTime = DateTime.UtcNow;
            string hmacString = ComputeHmacString(_secretKey, clientIP: "correct client IP", requestTime: requestTime);
            string authHeader = CombineAuthenticationHeader(hmacString, clientIP: "wrong client IP", requestTime: requestTime);

            _request.Headers.Add("Authorization", authHeader);

            // Act.
            _filter.OnAuthorization(_actionContext);

            // Assert.
            Assert.AreEqual(HttpStatusCode.Unauthorized, _actionContext.Response.StatusCode);
        }

        [Test]
        public void OnAuthorization_EncryptedNonceDiffersFromNonceInHeaderParams_AccessDenied()
        {
            // Arrange.
            DateTime requestTime = DateTime.UtcNow;
            string hmacString = ComputeHmacString(_secretKey, nonce: "correct nounce", requestTime: requestTime);
            string authHeader = CombineAuthenticationHeader(hmacString, nonce: "wrong nonce", requestTime: requestTime);

            _request.Headers.Add("Authorization", authHeader);

            // Act.
            _filter.OnAuthorization(_actionContext);

            // Assert.
            Assert.AreEqual(HttpStatusCode.Unauthorized, _actionContext.Response.StatusCode);
        }

        [Test]
        public void OnAuthorization_WrongRequestTimeStamp_AccessDenied()
        {
            // Arrange.
            DateTime requestTime = DateTime.UtcNow;
            DateTime wrongRequestTime = DateTime.UtcNow.AddHours(1);
            string hmacString = ComputeHmacString(_secretKey, requestTime: requestTime);
            string authHeader = CombineAuthenticationHeader(_secretKey, requestTime: wrongRequestTime);

            _request.Headers.Add("Authorization", authHeader);

            // Act.
            _filter.OnAuthorization(_actionContext);

            // Assert.
            Assert.AreEqual(HttpStatusCode.Unauthorized, _actionContext.Response.StatusCode);
        }

        [Test]
        public void OnAuthorization_WrongHttpMethod_ThreadCurrentPrincipalIdentityIsNotNull()
        {
            // Arrange.
            DateTime requestTime = DateTime.UtcNow;
            string hmacString = ComputeHmacString(_secretKey, requestTime: requestTime, requestMethod: "POST");
            string authHeader = CombineAuthenticationHeader(hmacString, requestTime: requestTime);

            _request.Headers.Add("Authorization", authHeader);
            _request.Method = HttpMethod.Get;

            // Act.
            _filter.OnAuthorization(_actionContext);

            // Assert.
            Assert.AreEqual(HttpStatusCode.Unauthorized, _actionContext.Response.StatusCode);
        }

        [Test]
        public void OnAuthorization_CorrectHmac_ThreadCurrentPrincipalIdentityIsNotNull()
        {
            // Arrange.
            DateTime requestTime = DateTime.UtcNow;
            string hmacString = ComputeHmacString(_secretKey, requestTime: requestTime);
            string authHeader = CombineAuthenticationHeader(hmacString, requestTime: requestTime);

            _request.Headers.Add("Authorization", authHeader);

            // Act.
            _filter.OnAuthorization(_actionContext);

            // Assert.
            Assert.AreEqual("127.0.0.1", Thread.CurrentPrincipal.Identity.Name);
        }

        private string ComputeHmacString(
            string secretKey,
            DateTime? requestTime = null,
            string clientIP = "127.0.0.1",
            string requestMethod = "GET",
            string requestUrl = "http://localhost:7878/customersetting/getcustomersetting",
            string nonce = "123abc")
        {
            string requestTimeStamp = ComputeRequestTimeStamp(requestTime ?? DateTime.UtcNow);
            string signatureRawData = $"{clientIP}{requestMethod}{HttpUtility.UrlEncode(requestUrl)}{requestTimeStamp}{nonce}";
            var secretKeyByteArray = Convert.FromBase64String(secretKey);
            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);
            string hmacString = string.Empty;

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);

                hmacString = Convert.ToBase64String(signatureBytes);
            }

            return hmacString;
        }

        private string CombineAuthenticationHeader(
            string hmacString,
            string headerScheme = "amx",
            string clientIP = "127.0.0.1",
            DateTime? requestTime = null,
            string nonce = "123abc")
        {
            return $"{headerScheme} {clientIP}:{hmacString}:{nonce}:{ComputeRequestTimeStamp(requestTime ?? DateTime.UtcNow)}";
        }

        private string ComputeRequestTimeStamp(DateTime checkingTime)
        {
            var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = checkingTime - epochStart;

            return Convert.ToUInt64(timeSpan.TotalSeconds).ToString();
        }
    }
}