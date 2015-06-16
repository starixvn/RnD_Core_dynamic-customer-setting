namespace Fanex.CustomerSetting.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Core.Criteria.Customer;
    using Fanex.CustomerSetting.Core.Handlers;
    using Fanex.CustomerSetting.Core.Messages;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Models.Params;
    using Fanex.CustomerSetting.Core.Results;
    using Fanex.CustomerSetting.Core.Results.Impl;
    using Fanex.CustomerSetting.Core.Tasks;
    using Fanex.NAP.Framework.Data;

    public class CustomerService : ICustomerService
    {
        private readonly ISettingContext _settingContext;
        private readonly IValidationService _validationService;
        private readonly IProductService _productService;
        private readonly ISettingResolver _settingResolver;
        private readonly IDynamicRepository _dynamicRepository;

        public CustomerService(
            ISettingContext settingContext,
            IValidationService validationService,
            IProductService productService,
            ISettingResolver settingResolver,
            IDynamicRepository dynamicRepository)
        {
            _validationService = validationService ?? new ValidationService();
            _productService = productService ?? new ProductService();
            _settingResolver = settingResolver ?? new SettingResolver();
            _settingContext = settingContext ?? SettingContextManager.SettingContext;
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public ISetting GetCustomerSetting(SettingQuery settingQuery)
        {
            Check.Argument.IsNotNull(settingQuery, "settingQuery");
            Check.Argument.IsNotNegativeOrZero(settingQuery.CustomerId, "settingQuery.CustomerId");
            Check.Argument.IsNotNegativeOrZero((byte)settingQuery.CustomerLevel, "settingQuery.CustomerLevel");
            Check.Argument.IsNotNegativeOrZero(settingQuery.ProductId, "settingQuery.ProductId");

            var validationResult = _validationService.ValidateSettingQuery(settingQuery);
            if (!validationResult.IsSuccessful)
            {
                throw new Exception(validationResult.Message);
            }

            ISetting setting = new Setting();

            setting.Customer = _dynamicRepository.Get<Customer>(
                new CustomerQueryCriteria
                {
                    CustId = settingQuery.CustomerId
                });

            var productSettingTypes = _productService.GetProductSettingTypes(settingQuery.ProductId);
            setting.Product = new Product
            {
                Id = settingQuery.ProductId,
                SettingTypes = productSettingTypes
            };

            IList<SettingType> customerSettingTypes = new List<SettingType>();

            foreach (var productSettingType in productSettingTypes)
            {
                SettingPreTasks preTasks = _settingResolver.ResolveSettingPreTasks(productSettingType);
                if (preTasks != null)
                {
                    preTasks.PreGetSetting(settingQuery);
                }

                var settingTypeDataAccessFactory = _settingResolver.ResolveSettingDataAccessFactory(productSettingType);
                var settingTypeDataAccess = settingTypeDataAccessFactory.Create(settingQuery.CustomerLevel);
                var customerSettingType = settingTypeDataAccess.GetCustomerSettingType(_settingContext.Actor, settingQuery);
                customerSettingType.Id = productSettingType.Id;
                customerSettingType.SettingTemplate = productSettingType.SettingTemplate;

                SettingPostTasks postTasks = _settingResolver.ResolveSettingPostTasks(productSettingType);
                if (postTasks != null)
                {
                    customerSettingType = postTasks.PostGetSetting(settingQuery, customerSettingType);
                }

                ITranslator translator = _settingResolver.ResolverTranslator(productSettingType);
                if (translator != null)
                {
                    translator.Translate(customerSettingType);
                }

                customerSettingTypes.Add(customerSettingType);
            }

            setting.SettingTypes = customerSettingTypes;

            return setting;
        }

        public IResults UpdateCustomerSetting(SettingCommand settingCommand)
        {
            Check.Argument.IsNotNull(settingCommand, "settingCommand");
            Check.Argument.IsNotNull(settingCommand.Setting, "settingCommand.Setting");
            Check.Argument.IsNotNegativeOrZero(settingCommand.Setting.Customer.Id, "settingCommand.Setting.Customer.Id");
            Check.Argument.IsNotNegativeOrZero(settingCommand.Setting.Product.Id, "settingQuery.CustomerLevel");
            Check.Argument.IsNotNegativeOrZero(settingCommand.Setting.SettingTypes.Count(), "settingCommand.Setting.SettingTypes.Count");

            return UpdateSingleCustomerSetting(settingCommand);
        }

        private IResults UpdateSingleCustomerSetting(SettingCommand settingCommand)
        {
            var settingTypes = settingCommand.Setting.SettingTypes;
            var customer = settingCommand.Setting.Customer;
            IList<IResult> results = new List<IResult>();

            foreach (var settingType in settingTypes)
            {
                SettingPreTasks preTasks = _settingResolver.ResolveSettingPreTasks(settingType);
                if (preTasks != null)
                {
                    var preTaskResult = preTasks.PreUpdateSetting(customer, settingType, settingCommand.UplineLevel);
                    results.Add(preTaskResult);
                }

                var settingTypeDataAccessFactory = _settingResolver.ResolveSettingDataAccessFactory(settingType);
                var settingTypeDataAccess = settingTypeDataAccessFactory.Create(customer.Level);
                var updateSettingResult = settingTypeDataAccess.UpdateCustomerSetting(
                    new UpdateCustomerSettingParams
                    {
                        Customer = customer,
                        SettingContext = _settingContext,
                        SettingType = settingType,
                        UplineLevel = settingCommand.UplineLevel
                    });

                if (updateSettingResult.IsSuccessful)
                {
                    SettingPostTasks postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
                    if (postTasks != null)
                    {
                        updateSettingResult = postTasks.PostUpdateSetting(customer, settingType, settingCommand.UplineLevel, updateSettingResult);
                    }
                }

                IMessageBuilder messageBuilder = _settingResolver.ResolveMessageBuilder(settingType);
                if (messageBuilder != null)
                {
                    updateSettingResult = messageBuilder.Build(settingType, updateSettingResult);
                }

                results.Add(updateSettingResult);
            }

            return new Results(customer.Id, customer.UserName, results);
        }

        public IMultipleResults UpdateMultipleCustomerSetting(MultipleSettingCommand multipleSettingCommand)
        {
            Check.Argument.IsNotNull(multipleSettingCommand, "multipleSettingCommand");
            Check.Argument.IsNotNull(multipleSettingCommand.Setting, "multipleSettingCommand.Setting");
            Check.Argument.IsNotNegativeOrZero(multipleSettingCommand.Setting.Product.Id, "multipleSettingCommand.CustomerLevel");
            Check.Argument.IsNotNegativeOrZero(multipleSettingCommand.Setting.SettingTypes.Count(), "multipleSettingCommand.Setting.SettingTypes.Count");
            Check.Argument.IsNotNegativeOrZero(multipleSettingCommand.Customers.Count(), "multipleSettingCommand.Customers.Count");

            var resultsList = new List<IResults>();
            var setting = multipleSettingCommand.Setting;
            var customers = multipleSettingCommand.Customers;
            var settingTypes = multipleSettingCommand.Setting.SettingTypes;

            foreach (var settingType in settingTypes)
            {
                SettingPreTasks preTasks = _settingResolver.ResolveSettingPreTasks(settingType);
                if (preTasks != null)
                {
                    var preTaskResult = preTasks.PreUpdateMultipleSetting(customers, settingType, multipleSettingCommand.UplineLevel);
                    resultsList.AddRange(preTaskResult.ResultsList);
                }

                var settingTypeResults = new List<IResults>();
                foreach (var customer in customers)
                {
                    var updateSingleResult = UpdateSingleCustomerSetting(new SettingCommand
                    {
                        Setting = new Setting
                        {
                            Customer = customer,
                            Product = setting.Product,
                            SettingTypes = new List<SettingType>
                            {
                                settingType
                            }
                        },
                        UplineLevel = multipleSettingCommand.UplineLevel
                    });

                    settingTypeResults.Add(updateSingleResult);
                }

                IMultipleResults settingTypeMultipleResults = new MultipleResults(settingTypeResults);

                if (settingTypeMultipleResults.IsSuccessful)
                {
                    SettingPostTasks postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
                    if (postTasks != null)
                    {
                        settingTypeMultipleResults = postTasks.PostUpdateMultipleSetting(
                            customers,
                            settingType,
                            multipleSettingCommand.UplineLevel,
                            settingTypeMultipleResults);
                    }
                }

                resultsList.AddRange(settingTypeMultipleResults.ResultsList);
            }

            return new MultipleResults(resultsList);
        }
    }
}