namespace Fanex.CustomerSetting.Tests.Services
{
    using Fanex.CustomerSetting.Services.Impl;
    using NUnit.Framework;

    [TestFixture]
    public class LanguageNameConverterTests
    {
        [Test]
        public void GetRefLanguageName_EmptyInput_ReturnExpectedResult()
        {
            Assert.AreEqual("en", LanguageNameConverter.GetRefLanguageName());
            Assert.AreEqual("en", LanguageNameConverter.GetRefLanguageName(string.Empty));
        }


        [Test]
        [TestCase("zh-tw", "ch")]
        [TestCase("zh-cn", "cs")]
        [TestCase("ja-jp", "jp")]
        [TestCase("th-th", "th")]
        [TestCase("ko-kr", "ko")]
        [TestCase("Vi-vn", "vn")]
        [TestCase("zH-tw", "ch")]
        [TestCase("zh-CN", "cs")]
        [TestCase("jA-Jp", "jp")]
        [TestCase("Th-Th", "th")]
        [TestCase("KO-KR", "ko")]
        [TestCase("Vi-Vn", "vn")]
        public void GetRefLanguageName_ValidInput_ReturnExpectedResult(string langName, string expectedResult)
        {
            Assert.AreEqual(expectedResult, LanguageNameConverter.GetRefLanguageName(langName));
        }
    }
}
