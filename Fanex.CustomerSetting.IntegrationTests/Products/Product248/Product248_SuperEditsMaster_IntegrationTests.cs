namespace Fanex.CustomerSetting.IntegrationTests.Products.Product248
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
    public class Product248_SuperEditsMaster_IntegrationTests : SuperEditsMaster_IntegrationTests
    {
        private IEnumerable<Action<ISetting>> _assertGotSettingTypes = new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType3.AssertGotCommission,
                    SettingTypesIntegration.SettingType4.AssertGotSMAProductStatus
                };

        private IEnumerable<Func<ISetting, ModifySettingTypeInfo>> _modifySettingTypes = new List<Func<ISetting, ModifySettingTypeInfo>>
                {
                    SettingTypesIntegration.SettingType3.ModifyMasterCommission,
                    SettingTypesIntegration.SettingType4.ModifySMAProductStatus
                };

        private Dictionary<byte, Action<ISettingItem, ISetting>> _assertUpdatedSettingTypes =
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                {
                    { SettingTypeInfo.CommissionSettingTypeId, SettingTypesIntegration.SettingType3.AssertUpdatedCommission },
                    { SettingTypeInfo.ProductStatusSettingTypeId, SettingTypesIntegration.SettingType4.AssertUpdatedSMAProductStatus }
                };

        [Test]
        public void Product248_SuperViewsMasterSetting()
        {
            ViewingMasterSetting(
                _assertGotSettingTypes,
                SettingInfo.SuperId,
                Level.Super,
                SettingInfo.MasterId,
                SettingInfo.MasterUserName,
                Product248Constants.ProductId,
                Product248Constants.NumOfSettingType);
        }

        [Test]
        public void Product248_CompanyViewsMasterSetting()
        {
            ViewingMasterSetting(
                _assertGotSettingTypes,
                SettingInfo.CompanyId,
                Level.Company,
                SettingInfo.MasterId,
                SettingInfo.MasterUserName,
                Product248Constants.ProductId,
                Product248Constants.NumOfSettingType);
        }

        [Test]
        public void Product248_SuperUpdatesMasterSetting()
        {
            SuperUpdatesMasterSetting(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.SuperId,
                Product248Constants.ProductId);
        }

        [Test]
        public void Product248_CompanyUpdatesMasterSetting()
        {
            CompanyUpdatesMasterSetting(
                _modifySettingTypes,
                _assertUpdatedSettingTypes,
                SettingInfo.CompanyId,
                Product248Constants.ProductId);
        }

        [Test]
        public void Product248_SuperUpdatesMultipleMastersSetting()
        {
            SuperUpdatesMultipleMastersSetting(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.SuperId,
                Product248Constants.ProductId);
        }
    }
}
