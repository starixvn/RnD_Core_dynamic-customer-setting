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
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(settingContext.Actor, nameof(settingContext.Actor));

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
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNull(command.Setting, nameof(command.Setting));
            Check.Argument.IsNotNull(command.Setting.SettingTypes, nameof(command.Setting.SettingTypes));
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
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNull(command.Setting, nameof(command.Setting));
            Check.Argument.IsNotNull(command.Setting.SettingTypes, nameof(command.Setting.SettingTypes));
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

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
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNull(command.Setting, nameof(command.Setting));
            Check.Argument.IsNotNull(command.Setting.SettingTypes, nameof(command.Setting.SettingTypes));

            var settingTypeResults = new List<SettingResult>();

            command.Setting.SettingTypes
                .Where(settingType => settingType.Id != StatusSettingTypeValue)
                .ToList()
                .ForEach(settingType =>
                {
                    var updateSettingTypeResult = UpdateCustomerSettingType(
                               command.Setting.CustomerId,
                               command.Setting.CustomerInfo,
                               command.Setting.CustomerLevel,
                               settingType,
                               command.Setting.ProductId,
                               command.UplineLevel);

                    settingTypeResults.Add(updateSettingTypeResult);
                });

            if (!settingTypeResults.Exists(result => result.IsNotOk) && _settingContext.System != (byte)SystemType.ExternalNapBetSetting)
            {
                var productStatusResult = UpdateProductStatus(command);

                if (productStatusResult != null)
                {
                    settingTypeResults.Add(productStatusResult);
                }
            }

            return new UpdateSettingResult(settingTypeResults);
        }

        private SettingResult UpdateProductStatus(UpdateCustomerSettingCommand command)
        {
            SettingResult productStatusResult = null;
            var statusSettingType = command.Setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == StatusSettingTypeValue);

            if (statusSettingType != null)
            {
                productStatusResult = UpdateCustomerSettingType(
                    command.Setting.CustomerId,
                    command.Setting.CustomerInfo,
                    command.Setting.CustomerLevel,
                    statusSettingType,
                    command.Setting.ProductId,
                    command.UplineLevel);

                if (productStatusResult.IsOk)
                {
                    _customerProductStatusService.UpdateCustomerStatus(
                        command.Setting.CustomerId,
                        command.Setting.ProductId,
                        _settingContext.Actor.Id,
                        _settingContext.Actor.SubAccountId,
                        _settingContext.IP);
                }
            }
            else if (command.Setting.CustomerLevel.IsNotMemberLevel() || command.UplineLevel?.IsAgentLevel() == true)
            {
                throw new CustomerSettingException($"Missing Product Status of customer {command.Setting.CustomerId}.");
            }

            return productStatusResult;
        }

        private SettingResult UpdateCustomerSettingType(
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

        private IResult Validate(UpdateCustomerSettingCommand command)
        {
            return _validationService.ValidateUpdateSettingAction(command);
        }

        private void CheckSettingTypeIsValid(SettingType settingType)
        {
            var result = settingType.Validate();

            if (result.IsNotOk)
            {
                throw new CustomerSettingException(result.Message);
            }
        }

        private void WriteLog(
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