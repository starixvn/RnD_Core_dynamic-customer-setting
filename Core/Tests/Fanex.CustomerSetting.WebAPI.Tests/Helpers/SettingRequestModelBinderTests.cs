namespace Fanex.CustomerSetting.WebAPI.Tests.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Metadata.Providers;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.Routing;
    using Models;
    using Models.Enumerations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NSubstitute;
    using NUnit.Framework;
    using WebApi.Helpers;
    using WebApi.Models.Request;
    using SysValueProviders = System.Web.Http.ValueProviders;

    [TestFixture]
    public abstract class SettingRequestModelBinderTests<T> where T : UpdateSettingRequest
    {
        protected static Setting SampleSetting
        {
            get
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

                return new Setting(1, Level.Super, customerInfo, 255, null, new List<SettingType> { settingType });
            }
        }

        protected string JsonModel { get; set; }

        protected HttpRequestMessage Request { get; set; }

        protected HttpControllerContext ControllerContext { get; set; }

        protected HttpActionDescriptor ActionDescriptor { get; set; }

        protected HttpActionContext ActionContext { get; set; }

        protected SysValueProviders.IValueProvider ValueProvider { get; set; }

        protected ModelBindingContext BindingContext { get; set; }

        protected UpdateSettingRequestModelBinder<T> ModelBinder { get; set; }

        [SetUp]
        public void SetUp()
        {
            var data = new DataAnnotationsModelMetadataProvider();
            var modelMetadata = data.GetMetadataForType(null, typeof(T));

            ValueProvider = Substitute.For<SysValueProviders.IValueProvider>();
            BindingContext = new ModelBindingContext
            {
                ValueProvider = ValueProvider,
                ModelMetadata = modelMetadata
            };

            Request = Substitute.For<HttpRequestMessage>();
            ControllerContext = new HttpControllerContext(Substitute.For<HttpConfiguration>(), Substitute.For<IHttpRouteData>(), Request);
            ActionDescriptor = Substitute.For<HttpActionDescriptor>();
            ActionContext = new HttpActionContext(ControllerContext, ActionDescriptor);
            ActionContext.ControllerContext.Request = Request;

            ModelBinder = new UpdateSettingRequestModelBinder<T>();
        }

        [Test]
        public void BindModel_ActionContextIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            ActionContext = null;

            // Act and Assert.
            var ex = Assert.Throws<ArgumentNullException>(() => ModelBinder.BindModel(ActionContext, BindingContext));

            Assert.AreEqual("actionContext", ex.ParamName);
        }

        [Test]
        public void BindModel_BindingContextIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            BindingContext = null;

            // Act and Assert.
            var ex = Assert.Throws<ArgumentNullException>(() => ModelBinder.BindModel(ActionContext, BindingContext));

            Assert.AreEqual("bindingContext", ex.ParamName);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void BindModel_ActorIdIsNotPositive_ModelStateIsInValid(int actorId)
        {
            // Arrange.
            JsonModel = BuildJsonModel(actorId: actorId);
            Request.Content = new StringContent(JsonModel);

            // Act.
            ModelBinder.BindModel(ActionContext, BindingContext);

            // Assert.
            Assert.IsFalse(ActionContext.ModelState.IsValid);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void BindModel_UplineLevelIsOutOfRange_ModelStateIsInValid(int uplineLevel)
        {
            // Arrange.
            JsonModel = BuildJsonModel(uplineLevel: (byte)uplineLevel);
            Request.Content = new StringContent(JsonModel);

            // Act.
            ModelBinder.BindModel(ActionContext, BindingContext);

            // Assert.
            Assert.IsFalse(ActionContext.ModelState.IsValid);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(6)]
        public void BindModel_ActorLevelIsOutOfRange_ModelStateIsInValid(int actorLevel)
        {
            // Arrange.
            JsonModel = BuildJsonModel(actorLevel: (byte)actorLevel);
            Request.Content = new StringContent(JsonModel);

            // Act.
            ModelBinder.BindModel(ActionContext, BindingContext);

            // Assert.
            Assert.IsFalse(ActionContext.ModelState.IsValid);
        }

        [Test]
        public void BindModel_SubAccountIdIsNegative_ModelStateIsInValid()
        {
            // Arrange.
            JsonModel = BuildJsonModel(subAccountId: -1);
            Request.Content = new StringContent(JsonModel);

            // Act.
            ModelBinder.BindModel(ActionContext, BindingContext);

            // Assert.
            Assert.IsFalse(ActionContext.ModelState.IsValid);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(6)]
        public void BindModel_SystemIsOutOfRange_ModelStateIsInValid(int system)
        {
            // Arrange.
            JsonModel = BuildJsonModel(system: (byte)system);
            Request.Content = new StringContent(JsonModel);

            // Act.
            ModelBinder.BindModel(ActionContext, BindingContext);

            // Assert.
            Assert.IsFalse(ActionContext.ModelState.IsValid);
        }

        [Test]
        public void BindModel_ActorNameIsNull_ModelStateIsInValid()
        {
            // Arrange.
            JsonModel = BuildJsonModel(actorName: null);
            Request.Content = new StringContent(JsonModel);

            // Act.
            ModelBinder.BindModel(ActionContext, BindingContext);

            // Assert.
            Assert.IsFalse(ActionContext.ModelState.IsValid);
        }

        protected static string BuildJsonModel(
            int actorId = 1,
            string actorName = "sa",
            byte actorLevel = 2,
            int subAccountId = 2,
            byte? uplineLevel = 3,
            byte system = 2,
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
    }
}