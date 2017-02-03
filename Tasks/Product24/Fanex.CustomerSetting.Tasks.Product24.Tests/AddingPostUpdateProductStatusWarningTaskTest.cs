namespace Fanex.CustomerSetting.Tasks.Product24.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;
    using NSubstitute;
    using NUnit.Framework;
    using SettingType13.Models;

    public class AddingPostUpdateProductStatusWarningTaskTest
    {
        private const short ProductStatusSettingType = 13;

        private AddingPostUpdateProductStatusWarningTask _task;
        private UpdateSettingResult _settingResults;
        private List<SettingResult> _subResults;
        private ISettingContext _settingContext;
        private ISetting _setting;

        [SetUp]
        public void SetUp()
        {
            _task = new AddingPostUpdateProductStatusWarningTask();
            _subResults = new List<SettingResult>();
            _settingResults = new UpdateSettingResult(_subResults);
            _settingContext = Substitute.For<ISettingContext>();
            _settingContext.Actor = Substitute.For<User>();
            _settingContext.Actor.Level = Level.Master;
            _settingContext.System = (byte)SystemType.Alpha;
            _setting = CreateSampleSettingData();
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

            _setting.SettingTypes = _setting.SettingTypes.Where(settingType => settingType.Id != ProductStatusSettingType);

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
            SettingType productStatusSettingType = _setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == ProductStatusSettingType);

            productStatusSettingType.SettingItems = new List<ProductStatusItem>
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

            _subResults.Add(new SettingResult(-1, Arg.Any<string>(), -1, 1, ProductStatusSettingType, Arg.Any<string>()));

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

            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, ProductStatusSettingType, Arg.Any<string>()));
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

            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, ProductStatusSettingType, Arg.Any<string>()));
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.IsTrue(_settingResults.SubResults.Any(subResult => subResult.Code == AddingPostUpdateProductStatusWarningTask.SuccessfulProductStatusUpdateWarningCode));
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostUpdateCustomerSetting_EnableDownlineOnExternalSites_AddWarningToRemindUserUpdateMemberSettings(byte systemId)
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, ProductStatusSettingType, Arg.Any<string>()));
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

            _setting.SettingTypes = _setting.SettingTypes.Where(settingType => settingType.Id != ProductStatusSettingType);

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
            SettingType productStatusSettingType = _setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == ProductStatusSettingType);

            productStatusSettingType.SettingItems = new List<ProductStatusItem>
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

            _subResults.Add(new SettingResult(-1, Arg.Any<string>(), -1, 1, ProductStatusSettingType, Arg.Any<string>()));

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

            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, ProductStatusSettingType, Arg.Any<string>()));
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

            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, ProductStatusSettingType, Arg.Any<string>()));
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, command, _settingResults);

            // Assert.
            Assert.IsTrue(_settingResults.SubResults.Any(subResult => subResult.Code == AddingPostUpdateProductStatusWarningTask.SuccessfulProductStatusUpdateWarningCode));
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostMultipleUpdateCustomerSetting_EnableDownlineOnExternalSites_AddWarningToRemindUserUpdateMemberSettings(byte systemId)
        {
            // Arrange.
            UpdateCustomerSettingCommand command = Substitute.For<UpdateCustomerSettingCommand>(_setting, Arg.Any<Level>());

            _subResults.Add(new SettingResult(0, Arg.Any<string>(), 0, 1, ProductStatusSettingType, Arg.Any<string>()));
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
                    Id = ProductStatusSettingType,
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

            return setting;
        }
    }
}