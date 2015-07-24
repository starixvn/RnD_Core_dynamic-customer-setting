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
    public class Product255_SuperEditsMember_IntegrationTests : SuperEditsMember_IntegrationTests
    {
        private IEnumerable<Action<ISetting>> _assertGotSettingTypes = new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType1.AssertGotSuperPTOnMember,
                    SettingTypesIntegration.SettingType2.AssertGotBetLimit,
                    SettingTypesIntegration.SettingType3.AssertGotCommission
                };

        private IEnumerable<Func<ISetting, ModifySettingTypeInfo>> _modifySettingTypes = new List<Func<ISetting, ModifySettingTypeInfo>>
                {
                    SettingTypesIntegration.SettingType1.ModifySuperPTOnMember
                };

        private Dictionary<byte, Action<ISettingItem, ISetting>> _assertUpdatedSettingTypes =
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                {
                    { ProductInfo.PTSettingTypeId, SettingTypesIntegration.SettingType1.AssertUpdatedSuperPTOnMember }
                };

        [Test]
        public void Product255_SuperViewsMemberSetting()
        {
            SuperViewsMemberSetting(
                _assertGotSettingTypes,
                SettingInfo.SuperId, 
                Level.Super);
        }

        [Test]
        public void Product255_CompanyViewsMemberSettingBySuperView()
        {
            SuperViewsMemberSetting(
                _assertGotSettingTypes,
                SettingInfo.CompanyId, 
                Level.Company);
        }

        [Test]
        public void Product255_SuperUpdatesPTOnMember()
        {
            SuperUpdatesPTOnMember(_modifySettingTypes, _assertUpdatedSettingTypes);
        }

        [Test]
        public void Product255_CompanyUpdatesSuperPTOnMember()
        {
            CompanyUpdatesSuperPTOnMember(_modifySettingTypes, _assertUpdatedSettingTypes);
        }

        [Test]
        public void Product255_SuperUpdatesMultipleMastersSetting()
        {
            SuperUpdatesPTOnMultipleMembers(_modifySettingTypes, _assertUpdatedSettingTypes);
        }
    }
}
