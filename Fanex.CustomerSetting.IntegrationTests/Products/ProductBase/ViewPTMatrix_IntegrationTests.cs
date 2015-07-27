namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.CustomerSetting.IntegrationTests.Products.Product255;
using Fanex.CustomerSetting.Matrix.Services;
using Fanex.CustomerSetting.Matrix.Services.Impl;
using Fanex.Data.Configuration;
using NUnit.Framework;

    public class ViewPTMatrix_IntegrationTests
    {
        private ICustomerService _customerService;
        private IPTMatrixService _positionTakingMatrixService;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
            _customerService = new CustomerService();
            _positionTakingMatrixService = new PTMatrixService();
        }

        
        public void ViewSuperPTMatrix(
            byte productId = Product255Constants.ProductId,
            string superName = SettingInfo.SuperUserName)
        {
            var superLine = _customerService.GetCustomerLine(superName);
            var superPTMatrix = _positionTakingMatrixService.GetPTMatrix(productId, superName);


        }


    }
}
