namespace Fanex.CustomerSetting.Tasks.SettingType13.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.SettingType13.Models;
    using Models;
    using Models.Constants;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;
    using NSubstitute;
    using NUnit.Framework;

    public class AddSettingType13WarningsTest
    {
        private AddSettingType13Warnings _task;
        private UpdateSettingResult _settingResults;
        private List<SettingResult> _subResults;
        private ISettingContext _settingContext;
        private ISetting _setting;

        [SetUp]
        public void SetUp()
        {
            _task = new AddSettingType13Warnings();
            _subResults = new List<SettingResult>();
            _settingResults = new UpdateSettingResult(_subResults);
            _settingContext = Substitute.For<ISettingContext>();
            _settingContext.Actor = Substitute.For<User>();
            _settingContext.Actor.Level = Level.Master;
            _settingContext.System = (byte)SystemType.Alpha;
            _setting = CreateSampleSettingData();
        }

        [Test]
        public void PostQueryCustomerSetting_InputSettingContextIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            int customerId = 1;
            byte productId = 1;
            Level customerLevel = Level.Agent;
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, null);

            _settingContext = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, query, _setting);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostQueryCustomerSetting_InputQueryIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            CustomerSettingQuery query = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, query, _setting);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostQueryCustomerSetting_InputSettingIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            int customerId = 1;
            byte productId = 1;
            Level customerLevel = Level.Agent;
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, null);

            _setting = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, query, _setting);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostQueryCustomerSetting_DoesNotUpdateSettingType13_NoAddWarningMessage()
        {
            // Arrange.
            int customerId = 1;
            byte productId = 1;
            Level customerLevel = Level.Agent;
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, null);

            _setting.SettingTypes = _setting.SettingTypes.Where(settingType => settingType.Id != AddSettingType13Warnings.SettingType13Id);

            // Act.
            _task.Invoke(_settingContext, query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_SystemIsNap_NoAddWarningMessage()
        {
            // Arrange.
            int customerId = 1;
            byte productId = 1;
            Level customerLevel = Level.Agent;
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, null);

            _settingContext.System = (byte)SystemType.Nap;

            // Act.
            _task.Invoke(_settingContext, query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_DisplayModeIsReadonly_NoAddWarningMessage()
        {
            // Arrange.
            int customerId = 1;
            byte productId = 1;
            Level customerLevel = Level.Agent;
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, null);
            SettingType settingType13 = _setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == AddSettingType13Warnings.SettingType13Id);

            settingType13.DisplayMode = DisplayMode.ReadOnly;

            // Act.
            _task.Invoke(_settingContext, query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostQueryCustomerSetting_EditSettingType13OnExternalSites_AddWarningMessageWithCodeIsNegative9102(byte systemId)
        {
            // Arrange.
            int customerId = 1;
            byte productId = 1;
            Level customerLevel = Level.Agent;
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, null);

            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, query, _setting);

            // Assert.
            Assert.IsTrue(_setting?.WarningMessage?.Code == AddSettingType13Warnings.ProductStatusNoteForExternalSitesCode);
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostQueryCustomerSetting_EditSettingType13OnExternalSites_AddWarningNoteToUser(byte systemId)
        {
            // Arrange.
            int customerId = 1;
            byte productId = 1;
            Level customerLevel = Level.Agent;
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, null);

            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, query, _setting);

            // Assert.
            Assert.IsTrue(_setting?.WarningMessage?.Message == Resources.ProductStatusNoteForExternalSites);
        }

        [Test]
        public void PostUpdateCustomerSetting_InputSettingContextIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _settingContext = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostUpdateCustomerSetting_InputCommandIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostUpdateCustomerSetting_InputUpdateResultIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _settingResults = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostUpdateCustomerSetting_DoesNotUpdateSettingType13_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _setting.SettingTypes = _setting.SettingTypes.Where(settingType => settingType.Id != AddSettingType13Warnings.SettingType13Id);

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.IsEmpty(_settingResults.SubResults);
        }

        [Test]
        public void PostUpdateCustomerSetting_DoesNotEnableDownline_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());
            SettingType settingType13 = _setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == AddSettingType13Warnings.SettingType13Id);

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
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.IsEmpty(_settingResults.SubResults);
        }

        [Test]
        public void PostUpdateCustomerSetting_DoesNotContainUpdateSettingType13Result_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.IsEmpty(_settingResults.SubResults);
        }

        [Test]
        public void PostUpdateCustomerSetting_UpdateSettingType13Failed_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _subResults.Add(new SettingResult(-1, Arg.Any<string>(), -1, 1, AddSettingType13Warnings.SettingType13Id, Arg.Any<string>()));

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.AreEqual(1, _settingResults.SubResults.Count());
        }

        [Test]
        public void PostUpdateCustomerSetting_SystemIsNAP_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, AddSettingType13Warnings.SettingType13Id, Arg.Any<string>()));
            _settingContext.System = (byte)SystemType.Nap;

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.AreEqual(1, _settingResults.SubResults.Count());
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostUpdateCustomerSetting_EnableDownlineOnExternalSites_AddWarningMessage(byte systemId)
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, AddSettingType13Warnings.SettingType13Id, Arg.Any<string>()));
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.IsTrue(_settingResults.SubResults.Any(subResult => subResult.Code == AddSettingType13Warnings.SuccessfulProductStatusUpdateWarningCode));
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostUpdateCustomerSetting_EnableDownlineOnExternalSites_AddWarningToRemindUserUpdateMemberSettings(byte systemId)
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, AddSettingType13Warnings.SettingType13Id, Arg.Any<string>()));
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.IsTrue(_settingResults.SubResults.Any(subResult => subResult.Message == Resources.SuccessfulProductStatusUpdateWarning));
        }

        [Test]
        public void PostUpdateMultipleCustomersSetting_InputSettingContextIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _settingContext = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostUpdateMultipleCustomersSetting_InputCommandIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostUpdateMultipleCustomersSetting_InputUpdateResultIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _settingResults = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostUpdateMultipleCustomerSetting_DoesNotUpdateSettingType13_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _setting.SettingTypes = _setting.SettingTypes.Where(settingType => settingType.Id != AddSettingType13Warnings.SettingType13Id);

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.IsEmpty(_settingResults.SubResults);
        }

        [Test]
        public void PostMultipleUpdateCustomerSetting_DoesNotEnableDownline_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());
            SettingType settingType13 = _setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == AddSettingType13Warnings.SettingType13Id);

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
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.IsEmpty(_settingResults.SubResults);
        }

        [Test]
        public void PostMultipleUpdateCustomerSetting_DoesNotContainUpdateSettingType13Result_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.IsEmpty(_settingResults.SubResults);
        }

        [Test]
        public void PostMultipleUpdateCustomerSetting_UpdateSettingType13Failed_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _subResults.Add(new SettingResult(-1, Arg.Any<string>(), -1, 1, AddSettingType13Warnings.SettingType13Id, Arg.Any<string>()));

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.AreEqual(1, _settingResults.SubResults.Count());
        }

        [Test]
        public void PostMultipleUpdateCustomerSetting_SystemIsNAP_SubResultsOfUpdateResultDoesNotChange()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, AddSettingType13Warnings.SettingType13Id, Arg.Any<string>()));
            _settingContext.System = (byte)SystemType.Nap;

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.AreEqual(1, _settingResults.SubResults.Count());
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostMultipleUpdateCustomerSetting_EnableDownlineOnExternalSites_AddWarningMessage(byte systemId)
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, AddSettingType13Warnings.SettingType13Id, Arg.Any<string>()));
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.IsTrue(_settingResults.SubResults.Any(subResult => subResult.Code == AddSettingType13Warnings.SuccessfulProductStatusUpdateWarningCode));
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostMultipleUpdateCustomerSetting_EnableDownlineOnExternalSites_AddWarningToRemindUserUpdateMemberSettings(byte systemId)
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, AddSettingType13Warnings.SettingType13Id, Arg.Any<string>()));
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.IsTrue(_settingResults.SubResults.Any(subResult => subResult.Message == Resources.SuccessfulProductStatusUpdateWarning));
        }

        private ISetting CreateSampleSettingData()
        {
            ISetting setting = Substitute.For<ISetting>();

            setting.SettingTypes = new List<SettingType>
            {
                new SettingType
                {
                    Id = AddSettingType13Warnings.SettingType13Id,
                    DisplayMode = 1,
                    SettingItems = new List<ProductStatusItem>
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
                                IsDisabled = false
                            }
                        }
                    }
                }
            };
            setting.CustomerLevel = Level.Master;
            setting.WarningMessage = null;

            return setting;
        }
    }
}