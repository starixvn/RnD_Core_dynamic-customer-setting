namespace Fanex.CustomerSetting.Casino.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Exceptions;
    using Logging;

    public class UpdateCasinoSettingService : IUpdateCasinoSettingService
    {
        private const byte StatusSettingTypeValue = 4;
        private readonly ISettingContext _settingContext;
        private readonly IUpdateSettingService _updateSettingService;
        private readonly IValidationService _validationService;
        private readonly ICustomerProductStatusService _customerProductStatusService;
        private readonly ISettingTypeServiceFactory _settingTypeServiceFactory;
        private readonly ILogger _logger;

        public UpdateCasinoSettingService()
        {
        }

        public UpdateCasinoSettingService(ISettingContext settingContext)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");

            _settingContext = settingContext;
            _updateSettingService = new UpdateSettingService(settingContext);
            _validationService = new ValidationService(settingContext);
            _customerProductStatusService = new CustomerProductStatusService();
            _settingTypeServiceFactory = new SettingTypeServiceFactory();
            _logger = Logger.Log;
        }

        public UpdateCasinoSettingService(
            ISettingContext settingContext,
            IUpdateSettingService updateSettingService,
            IValidationService validationService,
            ICustomerProductStatusService customerProductStatusService,
            ISettingTypeServiceFactory settingTypeServiceFactory,
            ILogger logger)
        {
            _settingContext = settingContext;
            _updateSettingService = updateSettingService;
            _validationService = validationService;
            _customerProductStatusService = customerProductStatusService;
            _settingTypeServiceFactory = settingTypeServiceFactory;
            _logger = logger;
        }

        public IResult Update(UpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, "command");
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

            var validationResult = Validate(command);

            if (validationResult.IsNotOk)
            {
                return validationResult;
            }

            var customerCasinoStatus = _customerProductStatusService.GetCustomerStatus(command.Setting.CustomerId, command.Setting.ProductId);

            if (customerCasinoStatus.IsSynced && customerCasinoStatus.IsDownlineSynced)
            {
                return _updateSettingService.Update(command);
            }

            return UpdateUnsyncedCustomerSetting(command);
        }

        public IResult Update(MultipleUpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, "command");
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

            var customerLevel = command.Setting.CustomerLevel;
            var uplineLevel = command.UplineLevel;
            var customerIds = command.CustomerIds;
            var settingTypes = command.Setting.SettingTypes;
            var productId = command.Setting.ProductId;
            var results = new List<IResult>();

            foreach (var customerId in command.CustomerIds)
            {
                var updateCustomerSettingCommand = command.GetUpdateCustomerSettingCommand(customerId);
                var result = Update(updateCustomerSettingCommand);

                results.Add(result);
            }

            return new UpdateSettingResult(results);
        }

        public IResult UpdateUnsyncedCustomerSetting(UpdateCustomerSettingCommand command)
        {
            var settingTypes = command.Setting.SettingTypes;
            var customerId = command.Setting.CustomerId;
            var customerLevel = command.Setting.CustomerLevel;
            var uplineLevel = command.UplineLevel;
            var productId = command.Setting.ProductId;
            var settingTypeResults = new List<SettingResult>();

            settingTypes
                .Where(settingType => settingType.Id != StatusSettingTypeValue)
                .ToList()
                .ForEach(settingType =>
                {
                    var updateSettingTypeResult = UpdateCustomerSettingType(
                               customerId,
                               command.Setting.CustomerInfo,
                               customerLevel,
                               settingType,
                               productId,
                               uplineLevel);

                    settingTypeResults.Add(updateSettingTypeResult);
                });

            if (!settingTypeResults.Exists(result => result.IsNotOk))
            {
                var statusSettingType = settingTypes.FirstOrDefault(settingType => settingType.Id == StatusSettingTypeValue);

                if (statusSettingType != null)
                {
                    var productStatusResult = UpdateCustomerSettingType(
                        customerId,
                        command.Setting.CustomerInfo,
                        customerLevel,
                        statusSettingType,
                        productId,
                        uplineLevel);
                    settingTypeResults.Add(productStatusResult);

                    if (productStatusResult.IsOk)
                    {
                        _customerProductStatusService.UpdateCustomerStatus(
                            command.Setting.CustomerId,
                            command.Setting.ProductId,
                            _settingContext.Actor.SubAccountId,
                            _settingContext.Actor.Id,
                            _settingContext.IP);
                    }
                }
                else if (command.Setting.CustomerLevel.IsNotMemberLevel() || command.UplineLevel?.IsAgentLevel() == true)
                {
                    throw new CustomerSettingException($"Missing Product Status of customer {customerId}.");
                }
            }

            return new UpdateSettingResult(settingTypeResults);
        }

        protected SettingResult UpdateCustomerSettingType(
           int customerId,
           CustomerInfo customerInfo,
           Level customerLevel,
           SettingType settingType,
           byte productId,
           Level uplineLevel)
        {
            try
            {
                CheckSettingTypeIsValid(settingType);

                var settingTypeService = _settingTypeServiceFactory.GetInstance(settingType);
                var settingResult = settingTypeService.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, uplineLevel, productId, settingType);
                settingResult.CustomerName = customerInfo != null ? customerInfo.UserName : string.Empty;

                return settingResult;
            }
            catch (CustomerSettingException customerSettingException)
            {
                WriteLog(customerId, customerLevel, uplineLevel, productId, settingType, customerSettingException);

                return new SettingResult(Result.AccessDenied.Code, customerSettingException.OriginalMessage, 0, customerId, settingType.Id, settingType.Name);
            }
            catch (Exception ex)
            {
                WriteLog(customerId, customerLevel, uplineLevel, productId, settingType, new CustomerSettingException(ex.Message, ex));

                return new SettingResult(
                    Result.SystemError,
                    customerId,
                    settingType.Id,
                    settingType.Name);
            }
        }

        protected IResult Validate(UpdateCustomerSettingCommand command)
        {
            return _validationService.ValidateUpdateSettingAction(command);
        }

        protected void CheckSettingTypeIsValid(SettingType settingType)
        {
            var result = settingType.Validate();

            if (result.IsNotOk)
            {
                throw new CustomerSettingException(result.Message);
            }
        }

        protected void WriteLog(
           int customerId,
           Level customerLevel,
           Level uplineLevel,
           byte productId,
           SettingType customerSettingType,
           CustomerSettingException ex)
        {
            ex.SetSettingContextInfo("Update Setting Casino", _settingContext);
            ex.SetCustomerInfo(customerId, customerLevel, uplineLevel);
            ex.SetProductInfo(productId, customerSettingType);

            _logger.Error(ex.Message, ex);
        }
    }
}