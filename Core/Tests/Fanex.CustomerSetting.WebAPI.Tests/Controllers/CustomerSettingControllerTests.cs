namespace Fanex.CustomerSetting.WebAPI.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Text;
    using System.Threading;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using WebApi.Controllers;
    using WebApi.Models.Request;

    [TestFixture]
    public class CustomerSettingControllerTests : IDisposable
    {
        private const string BaseAddress = "http://dummyname/";
        private static Setting _sampleSetting;

        private bool disposedValue;

        private ISettingServiceFactory _serviceFactory;
        private IContainer _container;
        private HttpConfiguration _httpConfiguration;
        private HttpServer _httpServer;
        private HttpMessageInvoker _messageInvoker;
        private HttpRequestMessage _request;
        private CancellationTokenSource _cancellationToken;

        protected static Setting SampleSetting
        {
            get
            {
                if (_sampleSetting == null)
                {
                    var customerInfo = new CustomerInfo
                    {
                        UserName = "SA-RM",
                        FirstName = "---",
                        LastName = "---",
                        Phone = "-",
                        MobilePhone = "-",
                        Fax = "-",
                        Email = "test@test.com"
                    };
                    var settingItem = new SettingItemDefault
                    {
                        SportTypeId = 121,
                        SportTypeName = "Sport Type 121",
                        BetTypeId = 30001,
                        BetTypeName = "Bet Type 30001"
                    };
                    var settingType = new SettingType
                    {
                        Id = 1,
                        Name = "Setting Type 1",
                        SettingLevel = 1,
                        DisplayOrder = 1,
                        DisplayMode = 1,
                        SettingItems = new List<ISettingItem>
                    {
                        settingItem
                    }
                    };

                    _sampleSetting = new Setting(1, Level.Super, customerInfo, 1, null, new List<SettingType> { settingType });
                }

                return _sampleSetting;
            }
        }

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();

            _serviceFactory = Substitute.For<ISettingServiceFactory>();

            builder
                .Register(context => new CustomerSettingController(_serviceFactory))
                .As<CustomerSettingController>()
                .InstancePerRequest();
            _container = builder.Build();

            _httpConfiguration = new HttpConfiguration();
            _httpConfiguration.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            _httpConfiguration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            _httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(_container);

            _httpServer = new HttpServer(_httpConfiguration);
            _messageInvoker = new HttpMessageInvoker(_httpServer);
            _request = new HttpRequestMessage();
            _cancellationToken = new CancellationTokenSource();
        }

        [TearDown]
        public void TearDown()
        {
            _request.Dispose();
            _messageInvoker.Dispose();
            _httpServer.Dispose();
            _httpConfiguration.Dispose();
            _container.Dispose();
            _cancellationToken.Dispose();
        }

        [Test]
        public void PostSettingRequest_ParameterIsNull_ReturnPreconditionFailed()
        {
            // Arrange.
            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/postsettingrequest");
            _request.Method = HttpMethod.Post;
            _request.Content = new ObjectContent<QuerySettingRequest>(new QuerySettingRequest(), new JsonMediaTypeFormatter());

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                Assert.AreEqual(HttpStatusCode.PreconditionFailed, response.StatusCode);
            }
        }

        [Test]
        public void PostSettingRequest_ParameterIsValid_CallSettingServiceFactoryCreateQueryService()
        {
            // Arrange.
            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/postsettingrequest");
            _request.Method = HttpMethod.Post;
            _request.Content = new StringContent(BuildQuerySettingRequestJsonBody(), Encoding.UTF8, "application/json");

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                _serviceFactory.Received().CreateQueryService(Arg.Is<byte>(1), Arg.Is<byte>(1), Arg.Any<ISettingContext>());
            }
        }

        [Test]
        public void PostSettingRequest_ParameterIsValid_CallQuerySettingServiceGetCustomerSetting()
        {
            // Arrange.
            var querySettingService = Substitute.For<IQuerySettingService>();

            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/postsettingrequest");
            _request.Method = HttpMethod.Post;
            _request.Content = new StringContent(BuildQuerySettingRequestJsonBody(), Encoding.UTF8, "application/json");
            _serviceFactory.CreateQueryService(Arg.Is<byte>(1), Arg.Is<byte>(1), Arg.Any<ISettingContext>()).Returns(querySettingService);

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                querySettingService.Received().GetCustomerSetting(Arg.Is<CustomerSettingQuery>(
                    query => query.ProductId == 1 && query.CustomerId == 1 && query.CustomerLevel == Level.Super));
                querySettingService.ReceivedCalls();
            }
        }

        [Test]
        public void PostSettingRequest_ParameterIsValid_ReturnStatusOk()
        {
            // Arrange.
            var querySettingService = Substitute.For<IQuerySettingService>();

            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/postsettingrequest");
            _request.Method = HttpMethod.Post;
            _request.Content = new StringContent(BuildQuerySettingRequestJsonBody(), Encoding.UTF8, "application/json");
            _serviceFactory.CreateQueryService(Arg.Is<byte>(1), Arg.Is<byte>(1), Arg.Any<ISettingContext>()).Returns(querySettingService);

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Test]
        public void Post_ParameterIsInvalid_ReturnPreconditionFailed()
        {
            // Arrange.
            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/post");
            _request.Method = HttpMethod.Post;
            _request.Content = new ObjectContent<UpdateSettingRequest>(new UpdateSettingRequest(), new JsonMediaTypeFormatter());

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                Assert.AreEqual(HttpStatusCode.PreconditionFailed, response.StatusCode);
            }
        }

        [Test]
        public void Post_ParameterIsValid_CallSettingServiceFactoryCreateUpdateService()
        {
            // Arrange.
            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/post");
            _request.Method = HttpMethod.Post;
            _request.Content = new StringContent(BuildJsonModel());

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                _serviceFactory.Received().CreateUpdateService(Arg.Is<byte>(1), Arg.Is<byte>(1), Arg.Any<ISettingContext>());
            }
        }

        [Test]
        public void Post_ParameterIsValid_CallUpdateSettingServiceUpdateCustomerSetting()
        {
            // Arrange.
            var updateSettingService = Substitute.For<IUpdateSettingService>();

            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/post");
            _request.Method = HttpMethod.Post;
            _request.Content = new StringContent(BuildJsonModel());
            _serviceFactory.CreateUpdateService(Arg.Is<byte>(1), Arg.Is<byte>(1), Arg.Any<ISettingContext>()).Returns(updateSettingService);

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                updateSettingService.Received().Update(Arg.Is<UpdateCustomerSettingCommand>(
                    command => command.Setting.ProductId == 1 && command.Setting.CustomerId == 1 && command.Setting.CustomerLevel == Level.Super));
            }
        }

        [Test]
        public void Post_ParameterIsValid_ReturnStatusOk()
        {
            // Arrange.
            var updateSettingService = Substitute.For<IUpdateSettingService>();

            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/post");
            _request.Method = HttpMethod.Post;
            _request.Content = new StringContent(BuildJsonModel());
            _serviceFactory.CreateUpdateService(Arg.Is<byte>(1), Arg.Is<byte>(1), Arg.Any<ISettingContext>()).Returns(updateSettingService);

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Test]
        public void PostMultiple_ParameterIsInvalid_ReturnPreconditionFailed()
        {
            // Arrange.
            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/postmultiple");
            _request.Method = HttpMethod.Post;
            _request.Content = new ObjectContent<MultipleUpdateSettingRequest>(new MultipleUpdateSettingRequest(), new JsonMediaTypeFormatter());

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                Assert.AreEqual(HttpStatusCode.PreconditionFailed, response.StatusCode);
            }
        }

        [Test]
        public void PostMultiple_ParameterIsValid_CallSettingServiceFactoryCreateUpdateService()
        {
            // Arrange.
            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/postmultiple");
            _request.Method = HttpMethod.Post;
            _request.Content = new StringContent(BuildJsonModel(customerIds: "1", customerNames: "sa"));

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                _serviceFactory.Received().CreateUpdateService(Arg.Is<byte>(1), Arg.Is<byte>(1), Arg.Any<ISettingContext>());
            }
        }

        [Test]
        public void PostMultiple_ParameterIsValid_CallUpdateSettingServiceUpdateMultipleCustomerSetting()
        {
            // Arrange.
            var updateSettingService = Substitute.For<IUpdateSettingService>();

            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/postmultiple");
            _request.Method = HttpMethod.Post;
            _request.Content = new StringContent(BuildJsonModel(customerIds: "1", customerNames: "sa"));
            _serviceFactory.CreateUpdateService(Arg.Is<byte>(1), Arg.Is<byte>(1), Arg.Any<ISettingContext>()).Returns(updateSettingService);

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                updateSettingService.Received().Update(Arg.Any<MultipleUpdateCustomerSettingCommand>());
            }
        }

        [Test]
        public void PostMultiple_ParameterIsValid_ReturnStatusOk()
        {
            // Arrange.
            var updateSettingService = Substitute.For<IUpdateSettingService>();

            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/postmultiple");
            _request.Method = HttpMethod.Post;
            _request.Content = new StringContent(BuildJsonModel(customerIds: "1", customerNames: "sa"));
            _serviceFactory.CreateUpdateService(Arg.Is<byte>(1), Arg.Is<byte>(1), Arg.Any<ISettingContext>()).Returns(updateSettingService);

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        /// <summary>
        /// Cannot test the GetTemplate method since the HostingEnvironment.MapPath returns
        /// null in the context of unit test runner.
        /// </summary>
        public void GetTemplate_IsIe8IsFalse_ReturnIe8Template()
        {
            // Arrange.
            _request.RequestUri = new Uri(BaseAddress + "api/customersetting/gettemplate?isIe8=true");
            _request.Method = HttpMethod.Get;

            // Act and Assert.
            using (HttpResponseMessage response = _messageInvoker.SendAsync(_request, _cancellationToken.Token).Result)
            {
                string ie8Template = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Templates\customer-setting-ie8.htm");

                Assert.AreEqual(ie8Template, response.Content.ToString());
            }
        }

        private static string BuildQuerySettingRequestJsonBody()
        {
            var jsonBuilder = new StringBuilder();

            jsonBuilder.Append("{");
            jsonBuilder.Append("\"ActorId\": 1,");
            jsonBuilder.Append("\"ActorName\": \"sa\",");
            jsonBuilder.Append("\"ActorLevel\": 5,");
            jsonBuilder.Append("\"SubAccountId\": 5,");
            jsonBuilder.Append("\"System\": 1,");
            jsonBuilder.Append("\"ProductId\": 1,");
            jsonBuilder.Append("\"CustomerId\": 1,");
            jsonBuilder.Append("\"CustomerLevel\": 4");
            jsonBuilder.Append("}");

            return jsonBuilder.ToString();
        }

        private static string BuildJsonModel(
            int actorId = 1,
            string actorName = "sa",
            byte actorLevel = 5,
            int subAccountId = 5,
            byte? uplineLevel = null,
            byte system = 1,
            string customerIds = null,
            string customerNames = null,
            string setting = null)
        {
            var jsonBuilder = new StringBuilder();
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            setting = setting ?? JsonConvert.SerializeObject(SampleSetting, jsonSerializerSettings).Replace("\"", "\\\"");

            jsonBuilder.Append("{");
            jsonBuilder.AppendFormat("\"actorId\": {0},", actorId);
            jsonBuilder.AppendFormat("\"actorLevel\": {0},", actorLevel);
            jsonBuilder.AppendFormat("\"subAccountId\": {0},", subAccountId);
            jsonBuilder.AppendFormat("\"system\": {0},", system);
            jsonBuilder.AppendFormat("\"setting\": \"{0}\",", setting);

            if (!string.IsNullOrEmpty(actorName))
            {
                jsonBuilder.AppendFormat("\"actorName\": \"{0}\",", actorName);
            }

            if (uplineLevel.HasValue)
            {
                jsonBuilder.AppendFormat("\"uplineLevel\": {0},", uplineLevel);
            }

            if (!string.IsNullOrEmpty(customerIds))
            {
                jsonBuilder.AppendFormat("\"customerIds\": \"{0}\",", customerIds);
            }

            if (!string.IsNullOrEmpty(customerNames))
            {
                jsonBuilder.AppendFormat("\"customerNames\": \"{0}\",", customerNames);
            }

            jsonBuilder.Append("}");

            return jsonBuilder.ToString();
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    TearDown();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}