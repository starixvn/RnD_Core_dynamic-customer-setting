namespace Fanex.CustomerSetting.Tests.Utilz
{
    using Fanex.CustomerSetting.Utilz;
    using NUnit.Framework;

    [TestFixture]
    public class LanguageNameConverterTests
    {
        [TestCase("ZH-TW", "ch")]
        [TestCase("ZH-CN", "cs")]
        [TestCase("JA-JP", "jp")]
        [TestCase("TH-TH", "th")]
        [TestCase("EN-US", "en")]
        [TestCase("1111", "en")]
        [TestCase("", "en")]
        [TestCase(null, "en")]
        public void GetRefLanguageName_ReturnRemotingRefLanguage(string language, string refLanguage)
        {
            //// Act
            var actualRefLanguage = LanguageNameConverter.GetRefLanguageName(language);

            //// Assert
            Assert.AreEqual(refLanguage, actualRefLanguage);
        }
    }
}