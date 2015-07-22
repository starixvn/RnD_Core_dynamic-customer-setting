namespace Fanex.CustomerSetting.IntegrationTests.Products.Product255
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using Fanex.CustomerSetting.Models;
    using NUnit.Framework;
    using SettingTypesIntegration = Fanex.CustomerSetting.IntegrationTests.Products.ProductBase.SettingTypes;

    [TestFixture]
    public class Product255_CompanyEditsSuper_IntegrationTests : CompanyEditsSuper_IntegrationTests
    {
        [Test]
        public void Product255_CompanyViewsSuperSetting()
        {
            CompanyViewsSuperSetting(new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType1.AssertGotSuperPT,
                    SettingTypesIntegration.SettingType2.AssertGotBetLimit,
                    SettingTypesIntegration.SettingType3.AssertGotCommission,
                    SettingTypesIntegration.SettingType4.AssertGotSMAProductStatus
                });
        }

        [Test]
        public void Product255_CompanyUpdatesSuperSetting()
        {
            CompanyUpdatesSuperSetting(
                new List<Func<ISetting, ModifySettingTypeInfo>>
                {
                    SettingTypesIntegration.SettingType1.ModifySuperPT,
                    SettingTypesIntegration.SettingType2.ModifySuperBetLimit,
                    SettingTypesIntegration.SettingType3.ModifySuperCommission,
                    SettingTypesIntegration.SettingType4.ModifySMAProductStatus
                },
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                {
                    { ProductInfo.PTSettingTypeId, SettingTypesIntegration.SettingType1.AssertUpdatedSuperPT },
                    { ProductInfo.BetLimitSettingTypeId, SettingTypesIntegration.SettingType2.AssertUpdatedBetLimit },
                    { ProductInfo.CommissionSettingTypeId, SettingTypesIntegration.SettingType3.AssertUpdatedCommission },
                    { ProductInfo.ProductStatusSettingTypeId, SettingTypesIntegration.SettingType4.AssertUpdatedSMAProductStatus },
                }
                );
        }
    }
}