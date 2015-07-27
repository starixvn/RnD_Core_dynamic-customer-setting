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
    public class Product255_MasterEditsMember_IntegrationTests : MasterEditsMember_IntegrationTests
    {
        private IEnumerable<Action<ISetting>> _assertGotSettingTypes = new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType1.AssertGotMasterPTOnMember,
                    SettingTypesIntegration.SettingType2.AssertGotBetLimit,
                    SettingTypesIntegration.SettingType3.AssertGotCommission
                };

        private IEnumerable<Func<ISetting, ModifySettingTypeInfo>> _modifySettingTypes = new List<Func<ISetting, ModifySettingTypeInfo>>
                {
                    SettingTypesIntegration.SettingType1.ModifyMasterPTOnMember
                };

        private Dictionary<byte, Action<ISettingItem, ISetting>> _assertUpdatedSettingTypes =
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                {
                    { SettingTypeInfo.PTSettingTypeId, SettingTypesIntegration.SettingType1.AssertUpdatedMasterPTOnMember }
                };

        [Test]
        public void Product255_MasterViewsMemberSetting()
        {
            MasterViewsMemberSetting(
                _assertGotSettingTypes,
                SettingInfo.MasterId,
                Level.Master);
        }

        [Test]
        public void Product255_CompanyViewsMemberSettingByMasterView()
        {
            MasterViewsMemberSetting(
                _assertGotSettingTypes,
                SettingInfo.CompanyId,
                Level.Company);
        }

        [Test]
        public void Product255_MasterUpdatesPTOnMember()
        {
            MasterUpdatesPTOnMember(_modifySettingTypes, _assertUpdatedSettingTypes);
        }

        [Test]
        public void Product255_CompanyUpdatesMasterPTOnMember()
        {
            CompanyUpdatesMasterPTOnMember(_modifySettingTypes, _assertUpdatedSettingTypes);
        }

        [Test]
        public void Product255_MasterUpdatesPTOnMultipleMembers()
        {
            MasterUpdatesPTOnMultipleMembers(_modifySettingTypes, _assertUpdatedSettingTypes);
        }
    }
}