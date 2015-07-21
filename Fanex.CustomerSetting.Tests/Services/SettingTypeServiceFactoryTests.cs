namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using Fanex.CustomerSetting.Services.Impl;
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