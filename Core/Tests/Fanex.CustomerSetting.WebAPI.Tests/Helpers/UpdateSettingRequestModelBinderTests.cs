namespace Fanex.CustomerSetting.WebAPI.Tests.Helpers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    using Models;
    using Models.Enumerations;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using WebApi.Helpers;
    using WebApi.Models.Request;

    [TestFixture]
    public abstract class UpdateSettingRequestModelBinderTests<T> where T : UpdateSettingRequest
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

        protected T Model { get; set; }

        protected HttpRequestMessage Request { get; set; }

        protected HttpControllerContext ControllerContext { get; set; }

        protected HttpActionDescriptor ActionDescriptor { get; set; }

        protected HttpActionContext ActionContext { get; set; }

        protected ModelBindingContext BindingContext { get; set; }

        protected UpdateSettingRequestModelBinder<T> ModelBinder { get; set; }

        public virtual void SetUp()
        {
            ModelBinder = new UpdateSettingRequestModelBinder<T>();
            BindingContext = new ModelBindingContext();
            Request = Substitute.ForPartsOf<HttpRequestMessage>();
            ControllerContext = Substitute.ForPartsOf<HttpControllerContext>();
            ActionDescriptor = Substitute.ForPartsOf<HttpActionDescriptor>();
            ActionContext = new HttpActionContext(ControllerContext, ActionDescriptor);
            ActionContext.ControllerContext.Request = Request;
            Request.Content = new StringContent(JsonConvert.SerializeObject(Model));
        }
    }
}