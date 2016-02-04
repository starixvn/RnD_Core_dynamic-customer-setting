namespace Fanex.CustomerSetting.ServiceLocation.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using NSubstitute;
    using Models;
    using Services.Impl;
    using System.Linq;
    using Fanex.CustomerSetting.ServiceLocation;
    using Casino.Services;
    using static SettingServiceBootstraper;
    using static Products;

    [TestFixture]
    public class SettingServiceLocatorTest
    {
        private static readonly object[] SupportingQuerySettingServiceTypes =
        {
            new object[] { Sportsbook2, true, typeof(QuerySettingService) },
            new object[] { Sportsbook2, false, typeof(UpdateSettingService)},
            new object[] { Cockfighting, true, typeof(QuerySettingService)},
            new object[] { Cockfighting, false, typeof(UpdateSettingService) },
            new object[] { Cricket, true, typeof(QuerySettingService)},
            new object[] { Cricket, false,typeof(UpdateSettingService) },
            new object[] { AllBet, true, typeof(QueryCasinoSettingService) },
            new object[] { AllBet, false, typeof(UpdateCasinoSettingService)}
        };

        private static readonly IEnumerable<object> SupportingProductIds =
           SupportingQuerySettingServiceTypes.Select(item => ((object[])item)[0]).Distinct();

        private void SetUpServiceTypeConfig()
        {
            RegisterService(Sportsbook2, typeof(QuerySettingService), typeof(UpdateSettingService));
            RegisterService(Cockfighting, typeof(QuerySettingService), typeof(UpdateSettingService));
            RegisterService(Cricket, typeof(QuerySettingService), typeof(UpdateSettingService));
            RegisterService(AllBet, typeof(QueryCasinoSettingService), typeof(UpdateCasinoSettingService));
        }

        [Test]
        public void GetService_ProductIdIsNotInSupportingRanges_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            var productId = Arg.Is<int>(x => !SupportingProductIds.Contains(x));
            var isQueryAction = Arg.Any<bool>();

            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceLocator().GetService(productId, isQueryAction, Arg.Any<ISettingContext>()));
        }

        [Test, TestCaseSource("SupportingQuerySettingServiceTypes")]
        public void GetService_ValidProductIdAlways_ReturnValidService(int productId, bool isQueryAction, Type serviceType)
        {
            //// Arrange
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();
            SetUpServiceTypeConfig();

            //// Act
            var querySettingService = new SettingServiceLocator().GetService(productId, isQueryAction, settingContext);

            //// Assert
            Assert.AreEqual(querySettingService.GetType(), serviceType, productId.ToString(), null);
        }
    }
}
