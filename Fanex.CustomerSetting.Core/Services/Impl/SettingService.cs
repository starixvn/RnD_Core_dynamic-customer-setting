namespace Fanex.CustomerSetting.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Dtos.Results.Impl;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Handlers;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.SPObjects.Criteria.Customer;
    using Fanex.NAP.Framework.Data;

    public class SettingService : ISettingService
    {
        private readonly ISettingContext _settingContext;
        private readonly IValidationService _validationService;
        private readonly IProductService _productService;
        private readonly ISettingResolver _settingResolver;
        private readonly IDynamicRepository _dynamicRepository;

        public SettingService()
            : this(null, null, null, null, null)
        {
        }

        public SettingService(
            ISettingContext settingContext,
            IValidationService validationService,
            IProductService productService,
            ISettingResolver settingResolver,
            IDynamicRepository dynamicRepository)
        {
            _settingContext = settingContext ?? SettingContextManager.SettingContext;
            _validationService = validationService ?? new ValidationService();
            _productService = productService ?? new ProductService();
            _settingResolver = settingResolver ?? new SettingResolver(_settingContext);
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public ISetting GetCustomerSetting(SettingQuery settingQuery)
        {
            CheckSettingQueryArguments(settingQuery);
            var validationResult = _validationService.ValidateSettingQuery(settingQuery);
            if (!validationResult.IsSuccessful)
            {
                throw new MethodAccessException(validationResult.Message);
            }

            ISetting setting = new Setting();
            setting.Customer = GetCustomer(settingQuery.CustomerId, settingQuery.CustomerLevel);
            setting.Product = GetProduct(settingQuery.ProductId);
            setting.SettingTypes = GetCustomerSettingTypes(settingQuery, setting.Product);

            return setting;
        }

        private IList<SettingType> GetCustomerSettingTypes(SettingQuery settingQuery, Product product)
        {
            IList<SettingType> customerSettingTypes = new List<SettingType>();
            foreach (var productSettingType in product.SettingTypes)
            {
                DoPreGetSettingTasks(settingQuery, productSettingType);
                var customerSettingType = GetCustomerSettingType(settingQuery, productSettingType);
                customerSettingType = DoPostGetSettingTasks(settingQuery, productSettingType, customerSettingType);
                customerSettingTypes.Add(customerSettingType);
            }

            return customerSettingTypes;
        }

        private SettingType DoPostGetSettingTasks(
            SettingQuery settingQuery,
            SettingType productSettingType,
            SettingType customerSettingType)
        {
            SettingPostTasks postTasks = _settingResolver.ResolveSettingPostTasks(productSettingType);
            if (postTasks != null && !postTasks.GetType().GetMethod("PostGetSetting").IsVirtual)
            {
                customerSettingType = postTasks.PostGetSetting(settingQuery, customerSettingType);
            }

            return customerSettingType;
        }

        private SettingType GetCustomerSettingType(SettingQuery settingQuery, SettingType productSettingType)
        {
            ISettingDataAccessFactory settingDataAccessFactory = _settingResolver.ResolveSettingDataAccessFactory(productSettingType);
            ISettingDataAccess settingDataAccess = settingDataAccessFactory.Create(settingQuery.CustomerLevel);
            var customerSettingType = settingDataAccess.GetCustomerSettingType(_settingContext.Actor, settingQuery);
            customerSettingType.Id = productSettingType.Id;
            customerSettingType.SettingTemplate = productSettingType.SettingTemplate;
            return customerSettingType;
        }

        private void DoPreGetSettingTasks(SettingQuery settingQuery, SettingType productSettingType)
        {
            SettingPreTasks preTasks = _settingResolver.ResolveSettingPreTasks(productSettingType);
            if (preTasks != null && !preTasks.GetType().GetMethod("PreGetSetting").IsVirtual)
            {
                preTasks.PreGetSetting(settingQuery);
            }
        }

        private Product GetProduct(byte productId)
        {
            IEnumerable<SettingType> productSettingTypes = _productService.GetProductSettingTypes(productId);
            var product = new Product
            {
                Id = productId,
                SettingTypes = productSettingTypes
            };

            return product;
        }

        private Customer GetCustomer(int customerId, CustomerLevel customerLevel)
        {
            var customer = _dynamicRepository.Get<Customer>(
                new CustomerQueryCriteria
                {
                    CustId = customerId
                });

            customer.Level = customerLevel;

            return customer;
        }

        private void CheckSettingQueryArguments(SettingQuery settingQuery)
        {
            Check.Argument.IsNotNull(settingQuery, "settingQuery");
            Check.Argument.IsNotNegativeOrZero(settingQuery.CustomerId, "settingQuery.CustomerId");
            Check.Argument.IsNotNegativeOrZero((byte)settingQuery.CustomerLevel, "settingQuery.CustomerLevel");
            Check.Argument.IsNotNegativeOrZero(settingQuery.ProductId, "settingQuery.ProductId");
        }

        public IResults UpdateCustomerSetting(SettingCommand settingCommand)
        {
            CheckSettingCommandArguments(settingCommand);
            return UpdateSingleCustomerSetting(settingCommand);
        }

        private void CheckSettingCommandArguments(SettingCommand settingCommand)
        {
            Check.Argument.IsNotNull(settingCommand, "settingCommand");
            Check.Argument.IsNotNull(settingCommand.Setting, "settingCommand.Setting");
            Check.Argument.IsNotNegativeOrZero(settingCommand.Setting.Customer.Id, "settingCommand.Setting.Customer.Id");
            Check.Argument.IsNotNegativeOrZero(settingCommand.Setting.Product.Id, "settingQuery.CustomerLevel");
            Check.Argument.IsNotNegativeOrZero(settingCommand.Setting.SettingTypes.Count(), "settingCommand.Setting.SettingTypes.Count");
        }

        private IResults UpdateSingleCustomerSetting(SettingCommand settingCommand)
        {
            var settingTypes = settingCommand.Setting.SettingTypes;
            var customer = settingCommand.Setting.Customer;
            IList<IResult> results = new List<IResult>();

            foreach (var settingType in settingTypes)
            {
                var result = UpdateSettingType(settingCommand, customer, settingType);
                results.Add(result);
            }

            return new Results(customer.Id, customer.UserName, results);
        }

        private IResult UpdateSettingType(SettingCommand settingCommand, Customer customer, SettingType settingType)
        {
            var preTaskResult = DoPreUpdateSettingTasks(settingCommand, customer, settingType);
            if (!preTaskResult.IsSuccessful)
            {
                return preTaskResult;
            }

            var updateSettingResult = UpdateSetting(customer, settingType, settingCommand.UplineLevel);
            if (updateSettingResult.IsSuccessful)
            {
                updateSettingResult = DoPostUpdateSettingTasks(customer, settingType, settingCommand.UplineLevel, updateSettingResult);
            }

            return updateSettingResult;
        }

        private IResult DoPostUpdateSettingTasks(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult updateSettingResult)
        {
            SettingPostTasks postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
            if (postTasks != null && !postTasks.GetType().GetMethod("PostUpdateSetting").IsVirtual)
            {
                updateSettingResult = postTasks.PostUpdateSetting(customer, settingType, uplineLevel, updateSettingResult);
            }

            return updateSettingResult;
        }

        private IResult UpdateSetting(Customer customer, SettingType settingType, CustomerLevel? uplineLevel)
        {
            var settingTypeDataAccessFactory = _settingResolver.ResolveSettingDataAccessFactory(settingType);
            var settingTypeDataAccess = settingTypeDataAccessFactory.Create(customer.Level);
            var updateSettingResult = settingTypeDataAccess.UpdateCustomerSetting(
                new UpdateCustomerSettingParams
                {
                    Customer = customer,
                    SettingContext = _settingContext,
                    SettingType = settingType,
                    UplineLevel = uplineLevel
                });

            return updateSettingResult;
        }

        private IResult DoPreUpdateSettingTasks(SettingCommand settingCommand, Customer customer, SettingType settingType)
        {
            SettingPreTasks preTasks = _settingResolver.ResolveSettingPreTasks(settingType);
            if (preTasks != null && !preTasks.GetType().GetMethod("PreUpdateSetting").IsVirtual)
            {
                var preTaskResult = preTasks.PreUpdateSetting(customer, settingType, settingCommand.UplineLevel);
                return preTaskResult;
            }

            return Result.Success;
        }

        public IMultipleResults UpdateMultipleCustomerSetting(MultipleSettingCommand multipleSettingCommand)
        {
            CheckMultipleSettingCommandAgruments(multipleSettingCommand);

            var resultsList = new List<IResults>();
            var product = multipleSettingCommand.Setting.Product;
            var customerLevel = multipleSettingCommand.Setting.Customer.Level;
            var uplineLevel = multipleSettingCommand.UplineLevel;
            var customers = multipleSettingCommand.Customers;
            var settingTypes = multipleSettingCommand.Setting.SettingTypes;

            foreach (var settingType in settingTypes)
            {
                var settingTypeMultipleResults = UpdateSetingTypeForMultipleCustomers(customers, customerLevel, uplineLevel, product, settingType);
                resultsList.AddRange(settingTypeMultipleResults.ResultsList);
            }

            return new MultipleResults(resultsList);
        }

        private IMultipleResults UpdateSetingTypeForMultipleCustomers(
            IList<Customer> customers,
            CustomerLevel customerLevel,
            CustomerLevel? uplineLevel,
            Product product,
            SettingType settingType)
        {
            var preTasksResult = DoPreUpdateMultipleSettingTasks(customers, settingType, uplineLevel);
            if (!preTasksResult.IsSuccessful)
            {
                return preTasksResult;
            }

            var settingTypeResults = UpdateSettingTypeOfCustomers(customers, customerLevel, settingType, product, uplineLevel);
            IMultipleResults settingTypeMultipleResults = new MultipleResults(settingTypeResults);

            if (settingTypeMultipleResults.IsSuccessful)
            {
                settingTypeMultipleResults = DoPostUpdateMultipleSettingTasks(customers, settingType, uplineLevel, settingTypeMultipleResults);
            }

            return settingTypeMultipleResults;
        }

        private void CheckMultipleSettingCommandAgruments(MultipleSettingCommand multipleSettingCommand)
        {
            Check.Argument.IsNotNull(multipleSettingCommand, "multipleSettingCommand");
            Check.Argument.IsNotNull(multipleSettingCommand.Setting, "multipleSettingCommand.Setting");
            Check.Argument.IsNotNegativeOrZero(multipleSettingCommand.Setting.Product.Id, "multipleSettingCommand.CustomerLevel");
            Check.Argument.IsNotNegativeOrZero(multipleSettingCommand.Setting.SettingTypes.Count(), "multipleSettingCommand.Setting.SettingTypes.Count");
            Check.Argument.IsNotNegativeOrZero(multipleSettingCommand.Customers.Count(), "multipleSettingCommand.Customers.Count");
        }

        private IMultipleResults DoPostUpdateMultipleSettingTasks(
            IList<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IMultipleResults settingTypeMultipleResults)
        {
            SettingPostTasks postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
            if (postTasks != null && !postTasks.GetType().GetMethod("PostUpdateMultipleSetting").IsVirtual)
            {
                settingTypeMultipleResults = postTasks.PostUpdateMultipleSetting(customers, settingType, uplineLevel, settingTypeMultipleResults);
            }

            return settingTypeMultipleResults;
        }

        private List<IResults> UpdateSettingTypeOfCustomers(
            IList<Customer> customers,
            CustomerLevel customerLevel,
            SettingType settingType,
            Product product,
            CustomerLevel? uplineLevel)
        {
            var settingTypeResults = new List<IResults>();
            foreach (var customer in customers)
            {
                var singleSettingCommand = CreateSingleSettingCommand(customer.Id, customerLevel, settingType, product, uplineLevel);
                var updateSingleResult = UpdateSingleCustomerSetting(singleSettingCommand);
                settingTypeResults.Add(updateSingleResult);
            }

            return settingTypeResults;
        }

        private SettingCommand CreateSingleSettingCommand(
            int customerId,
            CustomerLevel customerLevel,
            SettingType settingType,
            Product product,
            CustomerLevel? uplineLevel)
        {
            var singleSettingCommand = new SettingCommand
            {
                Setting = new Setting
                {
                    Customer = new Customer
                    {
                        Id = customerId,
                        Level = customerLevel
                    },
                    Product = product,
                    SettingTypes = new List<SettingType>
                            {
                                settingType
                            }
                },
                UplineLevel = uplineLevel
            };

            return singleSettingCommand;
        }

        private IMultipleResults DoPreUpdateMultipleSettingTasks(
            IList<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            SettingPreTasks preTasks = _settingResolver.ResolveSettingPreTasks(settingType);
            if (preTasks != null && !preTasks.GetType().GetMethod("PreUpdateMultipleSetting").IsVirtual)
            {
                var preTaskResult = preTasks.PreUpdateMultipleSetting(customers, settingType, uplineLevel);
                return preTaskResult;
            }

            return MultipleResults.SuccessMultipleResults;
        }
    }
}