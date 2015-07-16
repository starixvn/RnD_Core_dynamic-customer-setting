namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LoggingServiceTests
    {
        private ILogger _logger;
        private ISettingContext _settingContext;
        private ILoggingService _loggingService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _logger = Substitute.For<ILogger>();
            _loggingService = new LoggingService(_settingContext, _logger);
        }

        ////[Test]
        ////public void LogQueryCustomerSettingTypeError_Always_CallLogError()
        ////{
        ////    //// Arrange
        ////    var customerId = 1;
        ////    byte productId = 1;
        ////    var uplineLevel = UserLevel.Super;
        ////    SettingType settingType = InstanceFactory.CreateProductSettingType(1, 1);
        ////    var expectedException = new Exception("Query Exception");

        ////    //// Act
        ////    _loggingService.LogQueryCustomerSettingTypeError(customerId, uplineLevel, productId, settingType, expectedException);

        ////    //// Assert
        ////    var expectedErrorMessage = "Query Customer Setting Type Exception\r\n"
        ////        + "Actor Id: 100\r\n"
        ////        + "Actor Name: Admin\r\n"
        ////        + "Actor Level: Company\r\n"
        ////        + "Sub Account Id: 0\r\n"
        ////        + "Language: en-US\r\n"
        ////        + "IP: 192.168.1.1\r\n"
        ////        + "System: 1\r\n"
        ////        + "Domain: Domain Name\r\n"

        ////        + "Customer Id: 1\r\n"
        ////        + "Upline Level: Super\r\n"

        ////        + "ProductId: 1\r\n"
        ////        + "Setting Type Id: 1\r\n"
        ////        + "Setting Type Name: SettingTypeName\r\n"
        ////        + "Setting Type Level: 1\r\n"
        ////        + "Setting Type Template: 1\r\n"

        ////        + "Exception Message: Query Exception\r\n";

        ////    _logger.Received().Error(
        ////        Arg.Is<string>(actualErrorMessage => actualErrorMessage == expectedErrorMessage),
        ////        Arg.Is<Exception>(actualException => actualException.Message == expectedException.Message));
        ////}

        ////[Test]
        ////public void LogUpdatingCustomerSettingTypeError_Always_CallLogError()
        ////{
        ////    //// Arrange
        ////    var customerId = 2;
        ////    byte productId = 2;
        ////    UserLevel uplineLevel = null;
        ////    SettingType settingType = InstanceFactory.CreateProductSettingType(2, 2);
        ////    var expectedException = new Exception("Updating Exception");

        ////    //// Act
        ////    _loggingService.LogUpdatingCustomerSettingTypeError(customerId, uplineLevel, productId, settingType, expectedException);

        ////    //// Assert
        ////    var expectedErrorMessage = "Updating Customer Setting Type Exception\r\n"
        ////        + "Actor Id: 100\r\n"
        ////        + "Actor Name: Admin\r\n"
        ////        + "Actor Level: Company\r\n"
        ////        + "Sub Account Id: 0\r\n"
        ////        + "Language: en-US\r\n"
        ////        + "IP: 192.168.1.1\r\n"
        ////        + "System: 1\r\n"
        ////        + "Domain: Domain Name\r\n"

        ////        + "Customer Id: 2\r\n"

        ////        + "ProductId: 2\r\n"
        ////        + "Setting Type Id: 2\r\n"
        ////        + "Setting Type Name: SettingTypeName\r\n"
        ////        + "Setting Type Level: 1\r\n"
        ////        + "Setting Type Template: 2\r\n"

        ////        + "Exception Message: Updating Exception\r\n";
        ////    _logger.Received().Error(
        ////        Arg.Is<string>(actualErrorMessage => actualErrorMessage == expectedErrorMessage),
        ////        Arg.Is<Exception>(actualException => actualException.Message == expectedException.Message));
        ////}

        ////[Test]
        ////public void LogUpdatingMultipleCustomersSettingTypeError_Always_CallLogError()
        ////{
        ////    //// Arrange
        ////    var customerIds = new List<int> { 1, 2, 3, 4 };
        ////    byte productId = 1;
        ////    var uplineLevel = UserLevel.Super;
        ////    SettingType settingType = InstanceFactory.CreateProductSettingType(1, 1);
        ////    var expectedException = new Exception("Updating Multiple Exception");

        ////    //// Act
        ////    _loggingService.LogUpdatingMultipleCustomersSettingTypeError(customerIds, uplineLevel, productId, settingType, expectedException);

        ////    //// Assert
        ////    var expectedErrorMessage = "Updating Multiple Customers Setting Type Exception\r\n"
        ////        + "Actor Id: 100\r\n"
        ////        + "Actor Name: Admin\r\n"
        ////        + "Actor Level: Company\r\n"
        ////        + "Sub Account Id: 0\r\n"
        ////        + "Language: en-US\r\n"
        ////        + "IP: 192.168.1.1\r\n"
        ////        + "System: 1\r\n"
        ////        + "Domain: Domain Name\r\n"

        ////        + "Customer Ids: 1,2,3,4\r\n"
        ////        + "Upline Level: Super\r\n"

        ////        + "ProductId: 1\r\n"
        ////        + "Setting Type Id: 1\r\n"
        ////        + "Setting Type Name: SettingTypeName\r\n"
        ////        + "Setting Type Level: 1\r\n"
        ////        + "Setting Type Template: 1\r\n"

        ////        + "Exception Message: Updating Multiple Exception\r\n";
        ////    _logger.Received().Error(
        ////        Arg.Is<string>(actualErrorMessage => actualErrorMessage == expectedErrorMessage),
        ////        Arg.Is<Exception>(actualException => actualException.Message == expectedException.Message));
        ////}

        ////[Test]
        ////public void LogValidatingUpdatingError_Always_CallLogError()
        ////{
        ////    //// Arrange
        ////    var customerId = 1;
        ////    byte productId = 1;
        ////    var uplineLevel = UserLevel.Super;
        ////    SettingType settingType = InstanceFactory.CreateProductSettingType(1, 1);
        ////    var expectedException = new Exception("Validating Updating Exception");

        ////    //// Act
        ////    _loggingService.LogValidatingUpdatingError(customerId, uplineLevel, productId, expectedException);

        ////    //// Assert
        ////    var expectedErrorMessage = "Validating Update Customer Setting Exception\r\n"
        ////        + "Actor Id: 100\r\n"
        ////        + "Actor Name: Admin\r\n"
        ////        + "Actor Level: Company\r\n"
        ////        + "Sub Account Id: 0\r\n"
        ////        + "Language: en-US\r\n"
        ////        + "IP: 192.168.1.1\r\n"
        ////        + "System: 1\r\n"
        ////        + "Domain: Domain Name\r\n"

        ////        + "Customer Id: 1\r\n"
        ////        + "Upline Level: Super\r\n"

        ////        + "ProductId: 1\r\n"

        ////        + "Exception Message: Validating Updating Exception\r\n";
        ////    _logger.Received().Error(
        ////        Arg.Is<string>(actualErrorMessage => actualErrorMessage == expectedErrorMessage),
        ////        Arg.Is<Exception>(actualException => actualException.Message == expectedException.Message));
        ////}

        ////[Test]
        ////public void LogValidatingUpdatingMultipleError_Always_CallLogError()
        ////{
        ////    //// Arrange
        ////    var customerIds = new List<int> { 1, 2, 3, 4 };
        ////    byte productId = 1;
        ////    var uplineLevel = UserLevel.Super;
        ////    SettingType settingType = InstanceFactory.CreateProductSettingType(1, 1);
        ////    var expectedException = new Exception("Updating Multiple Exception");

        ////    //// Act
        ////    _loggingService.LogValidatingUpdatingMultipleError(customerIds, uplineLevel, productId, expectedException);

        ////    //// Assert
        ////    var expectedErrorMessage = "Validating Update Multiple Customers Setting Exception\r\n"
        ////        + "Actor Id: 100\r\n"
        ////        + "Actor Name: Admin\r\n"
        ////        + "Actor Level: Company\r\n"
        ////        + "Sub Account Id: 0\r\n"
        ////        + "Language: en-US\r\n"
        ////        + "IP: 192.168.1.1\r\n"
        ////        + "System: 1\r\n"
        ////        + "Domain: Domain Name\r\n"

        ////        + "Customer Ids: 1,2,3,4\r\n"
        ////        + "Upline Level: Super\r\n"

        ////        + "ProductId: 1\r\n"

        ////        + "Exception Message: Updating Multiple Exception\r\n";
        ////    _logger.Received().Error(
        ////        Arg.Is<string>(actualErrorMessage => actualErrorMessage == expectedErrorMessage),
        ////        Arg.Is<Exception>(actualException => actualException.Message == expectedException.Message));
        ////}
    }
}