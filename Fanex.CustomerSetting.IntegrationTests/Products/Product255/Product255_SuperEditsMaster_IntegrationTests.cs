namespace Fanex.CustomerSetting.IntegrationTests.Products.Product255
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using NUnit.Framework;
    using SettingTypesIntegration = Fanex.CustomerSetting.IntegrationTests.Products.ProductBase.SettingTypes;

    [TestFixture]
    public class Product255_SuperEditsMaster_IntegrationTests : SuperEditsMaster_IntegrationTests
    {
        private IEnumerable<Action<ISetting>> _assertGotSettingTypes = new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType1.AssertGotMasterPT,
                    SettingTypesIntegration.SettingType2.AssertGotBetLimit,
                    SettingTypesIntegration.SettingType3.AssertGotCommission,
                    SettingTypesIntegration.SettingType4.AssertGotSMAProductStatus
                };

        private IEnumerable<Func<ISetting, ModifySettingTypeInfo>> _modifySettingTypes = new List<Func<ISetting, ModifySettingTypeInfo>>
                {
                    SettingTypesIntegration.SettingType1.ModifyMasterPT,
                    SettingTypesIntegration.SettingType2.ModifyMasterBetLimit,
                    SettingTypesIntegration.SettingType3.ModifyMasterCommission,
                    SettingTypesIntegration.SettingType4.ModifySMAProductStatus
                };

        private Dictionary<byte, Action<ISettingItem, ISetting>> _assertUpdatedSettingTypes =
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                {
                    { SettingTypeInfo.PTSettingTypeId, SettingTypesIntegration.SettingType1.AssertUpdatedMasterPT },
                    { SettingTypeInfo.BetLimitSettingTypeId, SettingTypesIntegration.SettingType2.AssertUpdatedBetLimit },
                    { SettingTypeInfo.CommissionSettingTypeId, SettingTypesIntegration.SettingType3.AssertUpdatedCommission },
                    { SettingTypeInfo.ProductStatusSettingTypeId, SettingTypesIntegration.SettingType4.AssertUpdatedSMAProductStatus },
                };

        [Test]
        public void Product255_SuperViewsMasterSetting()
        {
            ViewingMasterSetting(
                _assertGotSettingTypes,
                SettingInfo.SuperId,
                Level.Super);
        }

        [Test]
        public void Product255_CompanyViewsMasterSetting()
        {
            ViewingMasterSetting(
                _assertGotSettingTypes,
                SettingInfo.CompanyId,
                Level.Company);
        }

        [Test]
        public void Product255_SuperUpdatesMasterSetting()
        {
            SuperUpdatesMasterSetting(_modifySettingTypes, _assertUpdatedSettingTypes);
        }

        [Test]
        public void Product255_CompanyUpdatesMasterSetting()
        {
            CompanyUpdatesMasterSetting(_modifySettingTypes, _assertUpdatedSettingTypes);
        }

        [Test]
        public void Product255_SuperUpdatesMultipleMastersSetting()
        {
            SuperUpdatesMultipleMastersSetting(_modifySettingTypes, _assertUpdatedSettingTypes);
        }
    }
}