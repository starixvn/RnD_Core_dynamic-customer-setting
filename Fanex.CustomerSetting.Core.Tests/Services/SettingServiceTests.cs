namespace Fanex.CustomerSetting.Core.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Core.Handlers;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Services;
    using Fanex.CustomerSetting.Core.Services.Impl;
    using Fanex.CustomerSetting.Core.Tests.Models;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingServiceTests
    {
        private ISettingService _settingService;
        private IValidationService _validationService;
        private IProductService _productService;
        private ISettingContext _settingContext;
        private IDynamicRepository _dymanicRepository;
        private ISettingResolver _settingResolver;

        [SetUp]
        public void Setup()
        {
            _dymanicRepository = Substitute.For<IDynamicRepository>();
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _validationService = Substitute.For<IValidationService>();

            _settingService = new SettingService(
                _settingContext,
                _validationService,
                _productService,
                _settingResolver,
                _dymanicRepository);
        }
    }
}
