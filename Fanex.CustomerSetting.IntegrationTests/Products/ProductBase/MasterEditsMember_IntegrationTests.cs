namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.IntegrationTests.Products.Product255;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.Data.Configuration;
    using NUnit.Framework;

    public class MasterEditsMember_IntegrationTests
    {
        private IUpdateSettingService _updateSettingService;
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
        }

        protected void MasterViewsMemberSetting(
            IEnumerable<Action<ISetting>> assertGotSettingTypes,
            int actorId,
            Level actorLevel,
            int memberId = SettingInfo.MemberId,
            string memberName = SettingInfo.MemberUserName,
            byte productId = Product255Constants.ProductId,
            byte numOfSettingTypes = Product255Constants.NumOfSettingType)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "Actor Name" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new CustomerSettingQuery(memberId, Level.Member, productId, Level.Master);

            //// Act
            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(memberName, memberSetting.CustomerInfo.UserName);
            Assert.AreEqual(memberId, memberSetting.CustomerId);
            Assert.AreEqual(Level.Member, memberSetting.CustomerLevel);
            Assert.AreEqual(productId, memberSetting.ProductId);
            Assert.IsNotNullOrEmpty(memberSetting.ProductName);
            Assert.AreEqual(numOfSettingTypes - 1, memberSetting.SettingTypes.Count());

            foreach (Action<ISetting> assertGotSettingType in assertGotSettingTypes)
            {
                assertGotSettingType(memberSetting);
            }
        }

        protected void MasterUpdatesPTOnMember(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int actorId = SettingInfo.MasterId,
            byte productId = Product255Constants.ProductId,
            int memberId = SettingInfo.MemberId)
        {
            //// Update two time for auto pt and not auto pt
            MasterUpdatesPTOnMember(modifySettingTypes, assertUpdatedSettingTypes, actorId, Level.Master, memberId, productId);
        }

        protected void CompanyUpdatesMasterPTOnMember(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int actorId = SettingInfo.CompanyId,
            byte productId = Product255Constants.ProductId,
            int memberId = SettingInfo.MemberId)
        {
            //// Update two time for auto pt and not auto pt
            MasterUpdatesPTOnMember(modifySettingTypes, assertUpdatedSettingTypes, actorId, Level.Company, memberId, productId);
        }

        private void MasterUpdatesPTOnMember(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int actorId,
            Level actorLevel,
            int memberId,
            byte productId)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new CustomerSettingQuery(memberId, Level.Member, productId, Level.Master);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            var expectedSettingTypeData = new Dictionary<byte, ISettingItem>();

            foreach (var modifySettingType in modifySettingTypes)
            {
                var modifiedSettingTypeInfo = modifySettingType(memberSetting);
                expectedSettingTypeData.Add(modifiedSettingTypeInfo.SettingTypeId, modifiedSettingTypeInfo.SettingItem);
            }

            memberSetting.SettingTypes = memberSetting.SettingTypes.Take(1);

            //// Act
            var updateParam = new UpdateCustomerSettingCommand(memberSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.Update(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            foreach (var assertUpdatedSettingType in assertUpdatedSettingTypes)
            {
                var expectedSettingItem = expectedSettingTypeData[assertUpdatedSettingType.Key];
                assertUpdatedSettingType.Value(expectedSettingItem, newMemberSetting);
            }
        }

        protected void MasterUpdatesPTOnMultipleMembers(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int masterId = SettingInfo.MasterId,
            byte productId = Product255Constants.ProductId,
            IEnumerable<int> memberIds = null,
            IEnumerable<string> memberNames = null)
        {
            memberIds = memberIds ?? SettingInfo.MemberIds;
            memberNames = memberNames ?? SettingInfo.MemberNames;

            //// Arrange
            var actor = new User { Id = masterId, Level = Level.Master, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new CustomerSettingQuery(memberIds.First(), Level.Member, productId, Level.Master);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var expectedSettingTypeData = new Dictionary<byte, ISettingItem>();

            foreach (var modifySettingType in modifySettingTypes)
            {
                var modifiedSettingTypeInfo = modifySettingType(memberSetting);
                expectedSettingTypeData.Add(modifiedSettingTypeInfo.SettingTypeId, modifiedSettingTypeInfo.SettingItem);
            }

            memberSetting.SettingTypes = memberSetting.SettingTypes.Take(1);

            //// Act
            var updatingMultipleParam = new MultipleUpdateCustomerSettingCommand(memberSetting, memberIds, memberNames, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.Update(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            foreach (var memberId in memberIds)
            {
                querySettingParam.CustomerId = memberId;
                var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

                foreach (var assertUpdatedSettingType in assertUpdatedSettingTypes)
                {
                    var expectedSettingItem = expectedSettingTypeData[assertUpdatedSettingType.Key];
                    assertUpdatedSettingType.Value(expectedSettingItem, newMemberSetting);
                }
            }
        }
    }
}