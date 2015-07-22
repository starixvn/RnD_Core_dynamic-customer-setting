namespace Fanex.CustomerSetting.IntegrationTests.Products.Product249
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using Fanex.CustomerSetting.Models;
    using NUnit.Framework;
    using SettingTypesIntegration = Fanex.CustomerSetting.IntegrationTests.Products.ProductBase.SettingTypes;

    [TestFixture]
    public class Product249_CompanyEditsSuper_IntegrationTests : CompanyEditsSuper_IntegrationTests
    {
        [Test]
        public void Product249_CompanyViewsSuperSetting()
        {
            CompanyViewsSuperSetting(new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType2.AssertGotBetLimit,
                    SettingTypesIntegration.SettingType4.AssertGotSMAProductStatus
                },
            Product249Constants.ProductId,
            Product249Constants.ProductName,
            Product249Constants.NumOfSettingType);
        }

        [Test]
        public void Product249_CompanyUpdatesSuperSetting()
        {
            CompanyUpdatesSuperSetting(
                new List<Func<ISetting, ModifySettingTypeInfo>>
                    {
                        SettingTypesIntegration.SettingType2.ModifySuperBetLimit,
                        SettingTypesIntegration.SettingType4.ModifySMAProductStatus
                    },
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                    {
                        { ProductInfo.BetLimitSettingTypeId, SettingTypesIntegration.SettingType2.AssertUpdatedBetLimit },
                        { ProductInfo.ProductStatusSettingTypeId, SettingTypesIntegration.SettingType4.AssertUpdatedSMAProductStatus }
                    },
                Product249Constants.ProductId);
        }
    }
}
