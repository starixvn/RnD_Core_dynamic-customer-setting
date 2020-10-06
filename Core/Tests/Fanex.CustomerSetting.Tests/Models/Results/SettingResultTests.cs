namespace Fanex.CustomerSetting.Tests.Models.Results
{
    using Fanex.CustomerSetting.Models.Results;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class SettingResultTests
    {
        [Test]
        public void SettingResult_3InitialParamsAlways_Success()
        {
            Assert.DoesNotThrow(() => new SettingResult(0, string.Empty, new List<string>()));
        }

        [Test]
        public void SettingResult_4InitialParamsAlways_Success()
        {
            Assert.DoesNotThrow(() => new SettingResult(0, string.Empty, 1, new List<string>()));
        }
    }
}
