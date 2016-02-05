namespace Fanex.CustomerSetting.ServiceLocation.Tests
{
    using System;
    using Casino.Services;
    using Fanex.CustomerSetting.ServiceLocation;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using Services.Impl;
    using static Products;
    using static SettingServiceBootstraper;

    [TestFixture]
    public class SettingServiceBootstraperTest
    {
        [SetUp]
        public void Setup()
        {
            SettingServiceBootstraper.Clear();
        }

        //[Test]
        //public void RegisterService_ConfigIsNull_CannotBeAdded()
        //{
        //    //// Arrange
        //    SettingServiceConfiguration config = null;

        //    //// Act
        //    SettingServiceBootstraper.RegisterService(config);

        //    //// Assert
        //}
    }
}
