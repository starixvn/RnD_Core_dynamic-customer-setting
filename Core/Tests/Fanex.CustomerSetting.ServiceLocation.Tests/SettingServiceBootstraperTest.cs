namespace Fanex.CustomerSetting.ServiceLocation.Tests
{
    using Fanex.CustomerSetting.ServiceLocation;
    using NUnit.Framework;

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
