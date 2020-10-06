namespace Fanex.CustomerSetting.Tests.Models
{
    using Fanex.CustomerSetting.Models;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class UnderMaintainanceInfoTests
    {
        private UnderMaintainanceInfo _UMInfo;

        [Test]
        public void UnderMaintainanceInfo_Always_Success()
        {
            var beginTime = new DateTime(2020, 1, 1, 0, 0, 0);
            var endTime = new DateTime(2020, 1, 1, 1, 0, 0);

            Assert.DoesNotThrow(() => _UMInfo = new UnderMaintainanceInfo());
            Assert.DoesNotThrow(()=> _UMInfo = new UnderMaintainanceInfo(1, 1, 1, beginTime, endTime));
        }
    }
}
