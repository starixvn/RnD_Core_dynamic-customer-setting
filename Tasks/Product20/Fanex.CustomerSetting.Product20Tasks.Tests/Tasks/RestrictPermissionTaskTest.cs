namespace Fanex.CustomerSetting.Product20Tasks.Tests.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Tasks.Product20;
    using Fanex.CustomerSetting.Tasks.Product20.Criteria;
    using Fanex.CustomerSetting.Tasks.Product20.Models;
    using Fanex.CustomerSetting.Tasks.Product20.Resources;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class RestrictPermissionTaskTest
    {
        private IDynamicRepository _repository;
        private RestrictPermissionTask _task;
        private ISettingContext _settingContext;

        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<IDynamicRepository>();
            _repository.Fetch<SettingTypeRestrictionMode>(Arg.Any<SettingRestrictionModesCriteria>()).Returns(CreateSampleRestrictionData1());
            _task = new RestrictPermissionTask(_repository);
            _settingContext = Substitute.For<ISettingContext>();
            _settingContext.Actor = Substitute.For<User>();
            _settingContext.Actor.Level = Level.Master;
        }

        [Test]
        public void PostQueryCustomerSetting_ParametersAreNull_ThrowException()
        {
            // Arrange.
            CustomerSettingQuery query = null;
            ISetting setting = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, query, setting);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostQueryCustomerSetting_NoRestrictionConfigured_DoNothing()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)20, null);
            _repository.Fetch<SettingTypeRestrictionMode>(Arg.Any<SettingRestrictionModesCriteria>()).Returns(CreateSampleRestrictionData3());

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            int numberOfEditableSettingTypes = setting.SettingTypes.Count(settingType => settingType.DisplayMode == RestrictionModes.Editable);
            int originalNumberOfEditableSettingTypes = 3;
            Assert.AreEqual(originalNumberOfEditableSettingTypes, numberOfEditableSettingTypes);
        }

        [Test]
        public void PostQueryCustomerSetting_NoRestrictionConfigured_WarningMessageIsNull()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)20, null);
            _repository.Fetch<SettingTypeRestrictionMode>(Arg.Any<SettingRestrictionModesCriteria>()).Returns(CreateSampleRestrictionData3());

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            Assert.IsNull(setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_SettingTypesDoNotMatch_DoNothing()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData2();
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)20, null);

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            int numberOfEditableSettingTypes = setting.SettingTypes.Count(settingType => settingType.DisplayMode == RestrictionModes.Editable);
            int originalNumberOfEditableSettingTypes = 3;
            Assert.AreEqual(originalNumberOfEditableSettingTypes, numberOfEditableSettingTypes);
        }

        [Test]
        public void PostQueryCustomerSetting_SettingTypesDoNotMatch_WarningMessageIsNull()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData2();
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)20, null);

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            Assert.IsNull(setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_Contain3SettingTypesAndRestrictSettingType1AndSettingType5_OnlyOneSettingTypeIsEditable()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)20, null);

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            List<SettingType> editableSettingTypes = setting.SettingTypes.Where(settingType => settingType.DisplayMode == RestrictionModes.Editable).ToList();
            Assert.AreEqual(1, editableSettingTypes.Count);
        }

        [Test]
        public void PostQueryCustomerSetting_Contain3SettingTypesAndRestrictSettingType1AndSettingType5_SettingType2IsStillEditable()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)20, null);

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            SettingType settingType2 = setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == 2);
            Assert.AreEqual(RestrictionModes.Editable, settingType2?.DisplayMode ?? 0);
        }

        [Test]
        public void PostQueryCustomerSetting_HaveRestrictedSettingType_WarningMessageIsNotNull()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)20, null);

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            Assert.IsNotNull(setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_HaveRestrictedSettingType_WarningMessageCodeIs20029()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)20, null);

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            Assert.AreEqual(20029, setting.WarningMessage.Code);
        }

        [Test]
        public void PostQueryCustomerSetting_HaveRestrictedSettingType_WarningMessageContainsSummaryMessage()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)20, null);

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            StringAssert.Contains(Message.Msg_20029, setting.WarningMessage.Message);
        }

        [Test]
        public void PostQueryCustomerSetting_RestrictionUpdateSetsHiddenToSettingType5_NumberOfSettingTypesIsDecreased()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)20, null);
            _repository.Fetch<SettingTypeRestrictionMode>(Arg.Any<SettingRestrictionModesCriteria>()).Returns(CreateSampleRestrictionData2());

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            Assert.Greater(3, setting.SettingTypes.Count());
        }

        [Test]
        public void PostQueryCustomerSetting_RestrictionUpdateSetsHiddenToSettingType5_RemoveSettingType5()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)20, null);
            _repository.Fetch<SettingTypeRestrictionMode>(Arg.Any<SettingRestrictionModesCriteria>()).Returns(CreateSampleRestrictionData2());

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            SettingType settingType5 = setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == 5);
            Assert.IsNull(settingType5);
        }

        [Test]
        public void PreUpdateCustomerSetting_ParametersAreNull_ThrowException()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, command);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PreUpdateCustomerSetting_RestrictUpdateForSettingType1_RemoveSettingType1()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(setting, Arg.Any<Level>());

            // Act.
            _task.Invoke(_settingContext, command);

            // Assert.
            SettingType settingType1 = setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == 1);
            Assert.IsNull(settingType1);
        }

        [Test]
        public void PreUpdateCustomerSetting_Contain3SettingTypesButRestrictedTwo_OnlyOneSettingTypeWillBeUpdated()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(setting, Arg.Any<Level>());

            // Act.
            _task.Invoke(_settingContext, command);

            // Assert.
            Assert.AreEqual(1, setting.SettingTypes.Count());
        }

        [Test]
        public void PreUpdateMultipleCustomersSetting_ParametersAreNull_ThrowException()
        {
            // Arrange.
            MultipleUpdateCustomerSettingCommand command = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, command);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PreUpdateMultipleCustomersSetting_RestrictUpdateForSettingType1_RemoveSettingType1()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            List<int> customerIds = new List<int> { 1 };
            List<string> customerNames = new List<string> { "username" };
            var command = Substitute.For<MultipleUpdateCustomerSettingCommand>(setting, customerIds, customerNames, Arg.Any<Level>());

            // Act.
            _task.Invoke(_settingContext, command);

            // Assert.
            SettingType settingType1 = setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == 1);
            Assert.IsNull(settingType1);
        }

        [Test]
        public void PreUpdateMultipleCustomersSetting_Contain3SettingTypesButRestrictedTwo_OnlyOneSettingTypeWillBeUpdated()
        {
            // Arrange.
            ISetting setting = CreateSampleSettingData1();
            List<int> customerIds = new List<int> { 1 };
            List<string> customerNames = new List<string> { "username" };
            var command = Substitute.For<MultipleUpdateCustomerSettingCommand>(setting, customerIds, customerNames, Arg.Any<Level>());

            // Act.
            _task.Invoke(_settingContext, command);

            // Assert.
            Assert.AreEqual(1, setting.SettingTypes.Count());
        }

        private List<SettingTypeRestrictionMode> CreateSampleRestrictionData1()
        {
            return new List<SettingTypeRestrictionMode>
            {
                new SettingTypeRestrictionMode
                {
                    SettingTypeId = 1,
                    Mode = 2,
                    ErrorCode = 20029
                },
                new SettingTypeRestrictionMode
                {
                    SettingTypeId = 2,
                    Mode = 1,
                    ErrorCode = 0
                },
                new SettingTypeRestrictionMode
                {
                    SettingTypeId = 5,
                    Mode = 2,
                    ErrorCode = 20029
                }
            };
        }

        private List<SettingTypeRestrictionMode> CreateSampleRestrictionData2()
        {
            return new List<SettingTypeRestrictionMode>
            {
                new SettingTypeRestrictionMode
                {
                    SettingTypeId = 1,
                    Mode = 2,
                    ErrorCode = 20029
                },
                new SettingTypeRestrictionMode
                {
                    SettingTypeId = 2,
                    Mode = 1,
                    ErrorCode = 0
                },
                new SettingTypeRestrictionMode
                {
                    SettingTypeId = 5,
                    Mode = 3,
                    ErrorCode = 20029
                }
            };
        }

        private List<SettingTypeRestrictionMode> CreateSampleRestrictionData3()
        {
            return new List<SettingTypeRestrictionMode>();
        }

        private ISetting CreateSampleSettingData1()
        {
            ISetting setting = Substitute.For<ISetting>();
            SettingType settingType1 = Substitute.For<SettingType>();
            SettingType settingType2 = Substitute.For<SettingType>();
            SettingType settingType5 = Substitute.For<SettingType>();

            settingType1.Id = 1;
            settingType1.DisplayMode = 1;

            settingType2.Id = 2;
            settingType2.DisplayMode = 1;

            settingType5.Id = 5;
            settingType5.DisplayMode = 1;

            setting.SettingTypes = new List<SettingType>
            {
                settingType1,
                settingType2,
                settingType5
            };
            setting.WarningMessage = null;
            setting.CustomerLevel = Level.Agent;

            return setting;
        }

        private ISetting CreateSampleSettingData2()
        {
            ISetting setting = Substitute.For<ISetting>();
            SettingType settingType3 = Substitute.For<SettingType>();
            SettingType settingType4 = Substitute.For<SettingType>();
            SettingType settingType6 = Substitute.For<SettingType>();

            settingType3.Id = 3;
            settingType3.DisplayMode = 1;

            settingType4.Id = 4;
            settingType4.DisplayMode = 1;

            settingType6.Id = 6;
            settingType6.DisplayMode = 1;

            setting.SettingTypes = new List<SettingType>
            {
                settingType3,
                settingType4,
                settingType6
            };
            setting.WarningMessage = null;
            setting.CustomerLevel = Level.Agent;

            return setting;
        }
    }
}