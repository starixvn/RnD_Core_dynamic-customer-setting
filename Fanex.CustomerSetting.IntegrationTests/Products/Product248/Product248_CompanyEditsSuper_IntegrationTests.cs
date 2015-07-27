namespace Fanex.CustomerSetting.IntegrationTests.Products.Product248
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
    public class Product248_CompanyEditsSuper_IntegrationTests : CompanyEditsSuper_IntegrationTests
    {
        [Test]
        public void Product248_CompanyViewsSuperSetting()
        {
            CompanyViewsSuperSetting(
                new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType3.AssertGotCommission,
                    SettingTypesIntegration.SettingType4.AssertGotSMAProductStatus
                },
                Product248Constants.ProductId,
                Product248Constants.NumOfSettingType);
        }

        [Test]
        public void Product248_CompanyUpdatesSuperSetting()
        {
            CompanyUpdatesSuperSetting(
                new List<Func<ISetting, ModifySettingTypeInfo>>
                    {
                        SettingTypesIntegration.SettingType3.ModifySuperCommission,
                        SettingTypesIntegration.SettingType4.ModifySMAProductStatus
                    },
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                    {
                        { SettingTypeInfo.CommissionSettingTypeId, SettingTypesIntegration.SettingType3.AssertUpdatedCommission },
                        { SettingTypeInfo.ProductStatusSettingTypeId, SettingTypesIntegration.SettingType4.AssertUpdatedSMAProductStatus }
                    },
                Product248Constants.ProductId);
        }
    }
}
