namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.IntegrationTests.Products.Product255;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.Data.Configuration;
    using NUnit.Framework;

    public class CompanyEditsSuper_IntegrationTests
    {
        private IUpdateSettingService _updateSettingService;
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
        }

        protected void CompanyViewsSuperSetting(
            IEnumerable<Action<ISetting>> assertGotSettingTypes,
            byte productId = Product255Constants.ProductId,
            string productName = Product255Constants.ProductName,
            byte numOfSettingTypes = Product255Constants.NumOfSettingType,
            int actorId = SettingInfo.CompanyId,
            int superId = SettingInfo.SuperId,
            string superUsername = SettingInfo.SuperUserName)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = Level.Company, Name = "Company Name" };
            _settingContext = new SettingContext(actor);
            _updateSettingService = new UpdateSettingService(_settingContext);
            _querySettingService = new QuerySettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(superId, Level.Super, productId);

            //// Act
            var superSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(superUsername, superSetting.CustomerInfo.UserName);
            Assert.AreEqual(superId, superSetting.CustomerId);
            Assert.AreEqual(Level.Super, superSetting.CustomerLevel);
            Assert.AreEqual(productId, superSetting.ProductId);
            ////Assert.AreEqual(productName, superSetting.ProductName);
            Assert.AreEqual(numOfSettingTypes, superSetting.SettingTypes.Count());

            foreach (Action<ISetting> assertGotSettingType in assertGotSettingTypes)
            {
                assertGotSettingType(superSetting);
            }
        }

        protected void CompanyUpdatesSuperSetting(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            byte productId = Product255Constants.ProductId,
            int companyId = SettingInfo.CompanyId,
            int superId = SettingInfo.SuperId)
        {
            //// Arrange
            var actor = new User { Id = companyId, Level = Level.Company, Name = "Company Name" };
            _settingContext = new SettingContext(actor);
            _updateSettingService = new UpdateSettingService(_settingContext);
            _querySettingService = new QuerySettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(superId, Level.Super, productId, null);

            var superSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            var expectedSettingTypeData = new Dictionary<byte, ISettingItem>();

            foreach (var modifySettingType in modifySettingTypes)
            {
                var modifiedSettingTypeInfo = modifySettingType(superSetting);
                expectedSettingTypeData.Add(modifiedSettingTypeInfo.SettingTypeId, modifiedSettingTypeInfo.SettingItem);
            }

            //// Act
            var updateParam = new UpdateSettingParam(superSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateCustomerSetting(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            var newSuperSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            foreach (var assertUpdatedSettingType in assertUpdatedSettingTypes)
            {
                var expectedSettingItem = expectedSettingTypeData[assertUpdatedSettingType.Key];
                assertUpdatedSettingType.Value(expectedSettingItem, newSuperSetting);
            }
        }
    }
}