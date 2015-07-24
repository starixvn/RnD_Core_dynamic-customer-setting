namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.IntegrationTests.Products.Product255;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Params;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.Data.Configuration;
    using NUnit.Framework;

    public class SuperEditsMember_IntegrationTests
    {
        private IUpdateSettingService _updateSettingService;
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
        }

        protected void SuperViewsMemberSetting(
            IEnumerable<Action<ISetting>> assertGotSettingTypes,
            int actorId,
            Level actorLevel,
            int memberId = SettingInfo.MemberId,
            string memberName = SettingInfo.MemberUserName,
            byte productId = Product255Constants.ProductId,
            string productName = Product255Constants.ProductName,
            byte numOfSettingTypes = Product255Constants.NumOfSettingType)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "Actor Name" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QueryCustomerSettingParam(memberId, Level.Member, productId, Level.Super);

            //// Act
            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(memberName, memberSetting.CustomerInfo.UserName);
            Assert.AreEqual(memberId, memberSetting.CustomerId);
            Assert.AreEqual(Level.Member, memberSetting.CustomerLevel);
            Assert.AreEqual(productId, memberSetting.ProductId);
            ////Assert.AreEqual(productName, memberSetting.ProductName);
            Assert.AreEqual(numOfSettingTypes - 1, memberSetting.SettingTypes.Count());

            foreach (Action<ISetting> assertGotSettingType in assertGotSettingTypes)
            {
                assertGotSettingType(memberSetting);
            }
            //// Master Comm
            ////Assert.GreaterOrEqual(memberCommissionItem.Upline.Upline.CommRate, 0);
            ////Assert.LessOrEqual(memberCommissionItem.Upline.Upline.CommRate, 0.01);
        }

        protected void SuperUpdatesPTOnMember(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int actorId = SettingInfo.SuperId,
            byte productId = Product255Constants.ProductId,
            int memberId = SettingInfo.MemberId)
        {
            SuperUpdatesPTOnMember(modifySettingTypes, assertUpdatedSettingTypes, actorId, Level.Super, memberId, productId);
        }

        protected void CompanyUpdatesSuperPTOnMember(
             IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int actorId = SettingInfo.CompanyId,
            byte productId = Product255Constants.ProductId,
            int memberId = SettingInfo.MemberId)
        {
            SuperUpdatesPTOnMember(modifySettingTypes, assertUpdatedSettingTypes, actorId, Level.Company, memberId, productId);
        }

        private void SuperUpdatesPTOnMember(
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

            var querySettingParam = new QueryCustomerSettingParam(memberId, Level.Member, productId, Level.Super);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var expectedSettingTypeData = new Dictionary<byte, ISettingItem>();

            foreach (var modifySettingType in modifySettingTypes)
            {
                var modifiedSettingTypeInfo = modifySettingType(memberSetting);
                expectedSettingTypeData.Add(modifiedSettingTypeInfo.SettingTypeId, modifiedSettingTypeInfo.SettingItem);
            }

            memberSetting.SettingTypes = memberSetting.SettingTypes.Take(1);

            //// Act
            var updateParam = new UpdateCustomerSettingParam(memberSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.Update(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(TestConfig.NumOfSleepSecondsWaitingForReplication); //// Wait for replication data.

            var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            foreach (var assertUpdatedSettingType in assertUpdatedSettingTypes)
            {
                var expectedSettingItem = expectedSettingTypeData[assertUpdatedSettingType.Key];
                assertUpdatedSettingType.Value(expectedSettingItem, newMemberSetting);
            }
        }

        protected void SuperUpdatesPTOnMultipleMembers(
             IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int superId = SettingInfo.SuperId,
            byte productId = Product255Constants.ProductId,
            IEnumerable<int> memberIds = null,
            IEnumerable<string> memberNames = null)
        {
            memberIds = memberIds ?? SettingInfo.MemberIds;
            memberNames = memberNames ?? SettingInfo.MemberNames;

            //// Arrange
            var actor = new User { Id = superId, Level = Level.Super, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);

            var querySettingParam = new QueryCustomerSettingParam(memberIds.First(), Level.Member, productId, Level.Super);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            var expectedSettingTypeData = new Dictionary<byte, ISettingItem>();

            foreach (var modifySettingType in modifySettingTypes)
            {
                var modifiedSettingTypeInfo = modifySettingType(memberSetting);
                expectedSettingTypeData.Add(modifiedSettingTypeInfo.SettingTypeId, modifiedSettingTypeInfo.SettingItem);
            }

            memberSetting.SettingTypes = memberSetting.SettingTypes.Take(1);

            //// Act
            var updatingMultipleParam = new UpdateMultipleCustomerSettingParam(memberSetting, memberIds, memberNames, querySettingParam.UplineLevel);
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