namespace Fanex.CustomerSetting.Tasks.SettingType13.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.SettingType13.Models;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;
    using NSubstitute;
    using NUnit.Framework;

    public class AddSettingType13UpdateWarningsTaskTest
    {
        private AddSettingType13UpdateWarningsTask _task;
        private UpdateSettingResult _settingResults;
        private List<SettingResult> _subResults;
        private ISettingContext _settingContext;
        private ISetting _setting;
        private UpdateCustomerSettingCommand _command;

        [SetUp]
        public void SetUp()
        {
            _task = new AddSettingType13UpdateWarningsTask();
            _subResults = new List<SettingResult>();
            _settingResults = new UpdateSettingResult(_subResults);
            _settingContext = Substitute.For<ISettingContext>();
            _settingContext.Actor = Substitute.For<User>();
            _settingContext.Actor.Level = Level.Master;
            _settingContext.System = (byte)SystemType.Alpha;
            _setting = SampleData.CreateSampleSettingData();
            _command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());
        }

        [Test]
        public void PostUpdateCustomerSetting_InputSettingContextIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            _settingContext = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostUpdateCustomerSetting_InputCommandIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            _command = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostUpdateCustomerSetting_InputUpdateResultIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            _settingResults = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostUpdateCustomerSetting_DoesNotUpdateSettingType13_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            _setting.SettingTypes = _setting.SettingTypes.Where(settingType => settingType.Id != AddSettingType13WarningTask.SettingType13Id);

            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.IsEmpty(_settingResults.SubResults);
        }

        [Test]
        public void PostUpdateCustomerSetting_DoesNotEnableDownline_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            SettingType settingType13 = _setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == AddSettingType13WarningTask.SettingType13Id);

            settingType13.SettingItems = new List<ProductStatusItem>
            {
                new ProductStatusItem
                {
                    SportTypeId = 1,
                    BetTypeId = 1,
                    IsDisabled = false,
                    Upline = new ProductStatusItem
                    {
                        IsDisabled = false
                    },
                    Downline = new ProductStatusItem
                    {
                        IsDisabled = true
                    }
                }
            };

            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.IsEmpty(_settingResults.SubResults);
        }

        [Test]
        public void PostUpdateCustomerSetting_DoesNotContainUpdateSettingType13Result_SubResultsOfUpdateResultDoesNotChange()
        {
            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.IsEmpty(_settingResults.SubResults);
        }

        [Test]
        public void PostUpdateCustomerSetting_UpdateSettingType13Failed_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            _subResults.Add(new SettingResult(-1, Arg.Any<string>(), -1, 1, AddSettingType13WarningTask.SettingType13Id, Arg.Any<string>()));

            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.AreEqual(1, _settingResults.SubResults.Count());
        }

        [Test]
        public void PostUpdateCustomerSetting_SystemIsNAP_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, AddSettingType13WarningTask.SettingType13Id, Arg.Any<string>()));
            _settingContext.System = (byte)SystemType.Nap;

            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.AreEqual(1, _settingResults.SubResults.Count());
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostUpdateCustomerSetting_EnableDownlineOnExternalSites_AddWarningMessage(byte systemId)
        {
            // Arrange.
            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, AddSettingType13WarningTask.SettingType13Id, Arg.Any<string>()));
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.IsTrue(_settingResults.SubResults.Any(subResult => subResult.Code == AddSettingType13UpdateWarningsTask.SuccessfulProductStatusUpdateWarningCode));
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostUpdateCustomerSetting_EnableDownlineOnExternalSites_AddWarningToRemindUserUpdateMemberSettings(byte systemId)
        {
            // Arrange.
            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, AddSettingType13WarningTask.SettingType13Id, Arg.Any<string>()));
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.IsTrue(_settingResults.SubResults.Any(subResult => subResult.Message == Resources.SuccessfulProductStatusUpdateWarning));
        }

        [Test]
        public void PostUpdateMultipleCustomersSetting_InputSettingContextIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            _settingContext = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostUpdateMultipleCustomersSetting_InputCommandIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            _command = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostUpdateMultipleCustomersSetting_InputUpdateResultIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            _settingResults = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostUpdateMultipleCustomerSetting_DoesNotUpdateSettingType13_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            _setting.SettingTypes = _setting.SettingTypes.Where(settingType => settingType.Id != AddSettingType13WarningTask.SettingType13Id);

            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.IsEmpty(_settingResults.SubResults);
        }

        [Test]
        public void PostMultipleUpdateCustomerSetting_DoesNotEnableDownline_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            SettingType settingType13 = _setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == AddSettingType13WarningTask.SettingType13Id);

            settingType13.SettingItems = new List<ProductStatusItem>
            {
                new ProductStatusItem
                {
                    SportTypeId = 1,
                    BetTypeId = 1,
                    IsDisabled = false,
                    Upline = new ProductStatusItem
                    {
                        IsDisabled = false
                    },
                    Downline = new ProductStatusItem
                    {
                        IsDisabled = true
                    }
                }
            };

            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.IsEmpty(_settingResults.SubResults);
        }

        [Test]
        public void PostMultipleUpdateCustomerSetting_DoesNotContainUpdateSettingType13Result_SubResultsOfUpdateResultDoesNotChange()
        {
            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.IsEmpty(_settingResults.SubResults);
        }

        [Test]
        public void PostMultipleUpdateCustomerSetting_UpdateSettingType13Failed_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            _subResults.Add(new SettingResult(-1, Arg.Any<string>(), -1, 1, AddSettingType13WarningTask.SettingType13Id, Arg.Any<string>()));

            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.AreEqual(1, _settingResults.SubResults.Count());
        }

        [Test]
        public void PostMultipleUpdateCustomerSetting_SystemIsNAP_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, AddSettingType13WarningTask.SettingType13Id, Arg.Any<string>()));
            _settingContext.System = (byte)SystemType.Nap;

            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.AreEqual(1, _settingResults.SubResults.Count());
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostMultipleUpdateCustomerSetting_EnableDownlineOnExternalSites_AddWarningMessage(byte systemId)
        {
            // Arrange.
            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, AddSettingType13WarningTask.SettingType13Id, Arg.Any<string>()));
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.IsTrue(_settingResults.SubResults.Any(subResult => subResult.Code == AddSettingType13UpdateWarningsTask.SuccessfulProductStatusUpdateWarningCode));
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostMultipleUpdateCustomerSetting_EnableDownlineOnExternalSites_AddWarningToRemindUserUpdateMemberSettings(byte systemId)
        {
            // Arrange.
            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, AddSettingType13WarningTask.SettingType13Id, Arg.Any<string>()));
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _command, _settingResults);

            // Assert.
            Assert.IsTrue(_settingResults.SubResults.Any(subResult => subResult.Message == Resources.SuccessfulProductStatusUpdateWarning));
        }
    }
}