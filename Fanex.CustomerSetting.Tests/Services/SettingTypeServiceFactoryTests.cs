namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingTypeServiceFactoryTests
    {
        [Test]
        public void GetInstance_SettingTypeIsNull_ThrowArgumentNullException()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>(), 
                delegate 
                {
                    new SettingTypeServiceFactory().GetInstance(null);
                });
        }
    }
}
