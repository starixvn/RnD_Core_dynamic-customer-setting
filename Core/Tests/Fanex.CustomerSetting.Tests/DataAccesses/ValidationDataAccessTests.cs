namespace Fanex.CustomerSetting.Tests.DataAccesses
{
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ValidationDataAccessTests
    {
        private readonly ValidationDataAccess validationDataAccess = new ValidationDataAccess();

        [SetUp]
        public void SetUp()
        {
            var subObjectDb = Substitute.For<IObjectDb>();
            subObjectDb.ExecuteNonQuery(Arg.Any<object>()).Returns(0);
            ObjectDbFactory.ObjectDb = (settingKey) => subObjectDb;
        }

        [Test]
        public void ValidatePermissionMultiple_Always_Success()
        {
            Assert.DoesNotThrow(()=> validationDataAccess.ValidatePermissionMultiple(0, string.Empty));
        }
    }
}
