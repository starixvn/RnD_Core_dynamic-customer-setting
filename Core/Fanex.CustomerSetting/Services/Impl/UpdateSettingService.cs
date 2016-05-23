namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Exceptions;
    using Logging;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;

    /// <summary>
    /// Service class for get single customer setting/ update single/multiple setting
    /// Setting includes list of setting types (pt, bet limit, commission ....)
    /// Customer can be super, master, agent, member
    /// So we call {Customer}{SettingType} => means SuperPositionTaking, MasterCommission ...
    /// </summary>
    public partial class UpdateSettingService : IUpdateSettingService
    {
        protected readonly ISettingContext SettingContext;
        protected readonly IValidationService ValidationService;
        protected readonly ITaskService TaskService;
        protected readonly ILogger Logger;
        protected readonly ISettingTypeServiceFactory SettingTypeServiceFactory;

        public UpdateSettingService(ISettingContext settingContext)
            : this(settingContext, null, null, null, null)
        {
        }

        public UpdateSettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            ITaskService pluggableTaskService = null,
            ILogger logger = null,
            ISettingTypeServiceFactory settingTypeServiceFactory = null)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");

            SettingContext = settingContext;
            ValidationService = validationService ?? new ValidationService(SettingContext);
            TaskService = pluggableTaskService ?? new TaskService(SettingContext);
            Logger = logger ?? Logging.Logger.Log;
            SettingTypeServiceFactory = settingTypeServiceFactory ?? new SettingTypeServiceFactory();
        }

        public UpdateSettingService()
        {
        }

        /// <summary>
        /// Updates the customer setting, includes list of setting types.
        /// </summary>
        /// <param name="command">The update parameters.</param>
        /// <returns>Result after update customer setting.</returns>
        public virtual IResult Update(UpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, "command");
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

            var validationResult = Validate(command);

            if (validationResult.IsNotOk)
            {
                return validationResult;
            }

            // Run plugged tasks before updating settings of customer.
            TaskService.Run(command.Setting.ProductId, command);

            IResult result = new UpdateSettingResult(UpdateCustomerSettingTypes(command));

            // Run plugged tasks after updating settings of customer.
            TaskService.Run(command.Setting.ProductId, command, result);

            return result;
        }

        protected virtual IResult Validate(UpdateCustomerSettingCommand command)
        {
            return ValidationService.ValidateUpdateSettingAction(command);
        }

        protected virtual IEnumerable<SettingResult> UpdateCustomerSettingTypes(UpdateCustomerSettingCommand command)
        {
            var settingTypes = command.Setting.SettingTypes;
            var customerId = command.Setting.CustomerId;
            var customerLevel = command.Setting.CustomerLevel;
            var uplineLevel = command.UplineLevel;
            var productId = command.Setting.ProductId;
            var settingTypeResults = new List<SettingResult>();
            SettingResult updateSettingTypeResult = null;

            foreach (var settingType in settingTypes)
            {
                updateSettingTypeResult = UpdateCustomerSettingType(
                    customerId,
                    command.Setting.CustomerInfo,
                    customerLevel,
                    settingType,
                    productId,
                    uplineLevel);

                settingTypeResults.Add(updateSettingTypeResult);
            }

            return settingTypeResults;
        }

        protected virtual SettingResult UpdateCustomerSettingType(
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

                TaskService.Run(productId, settingType.Id, customerId, customerLevel, uplineLevel, settingType);

                var settingTypeService = SettingTypeServiceFactory.GetInstance(settingType);
                var settingResult = settingTypeService.UpdateCustomerSettingType(SettingContext, customerId, customerLevel, uplineLevel, productId, settingType);
                settingResult.CustomerName = customerInfo != null ? customerInfo.UserName : string.Empty;

                TaskService.Run(productId, settingType.Id, customerId, customerLevel, uplineLevel, settingType, settingResult);

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

        protected virtual void CheckSettingTypeIsValid(SettingType settingType)
        {
            var result = settingType.Validate();

            if (result.IsNotOk)
            {
                throw new CustomerSettingException(result.Message);
            }
        }

        protected virtual void WriteLog(
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType customerSettingType,
            CustomerSettingException ex)
        {
            ex.SetSettingContextInfo("Update Setting", SettingContext);
            ex.SetCustomerInfo(customerId, customerLevel, uplineLevel);
            ex.SetProductInfo(productId, customerSettingType);

            Logger.Error(ex.Message, ex);
        }
    }
}