namespace Fanex.CustomerSetting.Casino.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Criteria;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services.Impl;
    using Data.Repository;
    using Exceptions;
    using Fanex.CustomerSetting.Services;
    using Logging;
    using Models;

    public class UpdateCasinoSettingService : IUpdateCasinoSettingService
    {
        private const byte StatusSettingTypeValue = 1;
        protected readonly ISettingContext _settingContext;
        protected readonly IUpdateSettingService _updateSettingService;
        protected readonly IValidationService _validationService;
        protected readonly IDynamicRepository _dynamicRepository;
        protected readonly ISettingTypeServiceFactory _settingTypeServiceFactory;
        protected readonly ILogger _logger;

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
            _dynamicRepository = new DynamicRepository();
            _settingTypeServiceFactory = new SettingTypeServiceFactory();
            _logger = Logger.Log;
        }

        public UpdateCasinoSettingService(
            ISettingContext settingContext,
            IUpdateSettingService updateSettingService,
            IValidationService validationService,
            IDynamicRepository dynamicRepository, 
            ISettingTypeServiceFactory settingTypeServiceFactory,
            ILogger logger)
        {
            _settingContext = settingContext;
            _updateSettingService = updateSettingService;
            _validationService = validationService;
            _dynamicRepository = dynamicRepository;
            _settingTypeServiceFactory = settingTypeServiceFactory;
            _logger = logger;
        }

        private CustomerCasinoStatus GetCustomerCasinoStatus(byte productId, int customerId)
        {
            var customerCasinoStatus = _dynamicRepository.Get<CustomerCasinoStatus>(
                 new GetCustomerCasinoStatusCriteria
                 {
                     ProductId = productId,
                     CustId = customerId
                 });

            return customerCasinoStatus;
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

            var customerCasinoStatus = GetCustomerCasinoStatus(command.Setting.ProductId, command.Setting.CustomerId);
            if (customerCasinoStatus == null || customerCasinoStatus.IsHidden)
            {
                throw new InvalidOperationException($"Cannot get setting for customer {command.Setting.CustomerId} due to hidden status on product {command.Setting.ProductId}");
            }

            if (customerCasinoStatus.IsSynced && customerCasinoStatus.IsDownlineSynced)
            {
                return _updateSettingService.Update(command);
            }

            return new UpdateSettingResult(UpdateCustomerSettingTypes(command));
        }

        private IResult Validate(UpdateCustomerSettingCommand command)
        {
            return _validationService.ValidateUpdateSettingAction(command);
        }

        protected IEnumerable<SettingResult> UpdateCustomerSettingTypes(UpdateCustomerSettingCommand command)
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
                settingTypeResults.Add(
                    UpdateCustomerSettingType(
                                   customerId,
                                   command.Setting.CustomerInfo,
                                   customerLevel,
                                   settingTypes.First(settingType => settingType.Id == StatusSettingTypeValue),
                                   productId,
                                   uplineLevel));
            }

            // TODO: Update IsSync

            return settingTypeResults;
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

        protected  void CheckSettingTypeIsValid(SettingType settingType)
        {
            var result = settingType.Validate();

            if (result.IsNotOk)
            {
                throw new CustomerSettingException(result.Message);
            }
        }

        protected  void WriteLog(
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType customerSettingType,
            CustomerSettingException ex)
        {
            ex.SetSettingContextInfo("Update Setting", _settingContext);
            ex.SetCustomerInfo(customerId, customerLevel, uplineLevel);
            ex.SetProductInfo(productId, customerSettingType);

            _logger.Error(ex.Message, ex);
        }

        //protected override List<SettingResult> UpdateSettingTypesForMultipleCustomers(MultipleUpdateCustomerSettingCommand command)
        //{
        //    var customerLevel = command.Setting.CustomerLevel;
        //    var uplineLevel = command.UplineLevel;
        //    var customerIds = command.CustomerIds;
        //    var settingTypes = command.Setting.SettingTypes;
        //    var productId = command.Setting.ProductId;
        //    var results = new List<SettingResult>();

        //    foreach (var customerId in command.CustomerIds)
        //    {
        //        var updateCustomerSettingCommand = command.GetUpdateCustomerSettingCommand(customerId);
        //        var result = Update(updateCustomerSettingCommand);

        //        results.Add(result as SettingResult);
        //    }

        //    return results;
        //}

        public IResult Update(MultipleUpdateCustomerSettingCommand command)
        {
            throw new NotImplementedException();
        }
    }
}