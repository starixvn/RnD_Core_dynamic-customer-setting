namespace Fanex.CustomerSetting.Tests.Models.Results
{
    using Fanex.CustomerSetting.Models.Constants;
    using Fanex.CustomerSetting.Models.Results;
    using NUnit.Framework;

    [TestFixture]
    public class ResultTests
    {
        [Test]
        public void Equals_Null_ReturnFalse()
        {
            Assert.IsFalse(Result.Success.Equals(null));
        }

        [Test]
        public void Equals_SameRef_ReturnTrue()
        {
            Assert.IsTrue(Result.Success.Equals(Result.Success));
        }

        [Test]
        public void Equals_DifRefSameValue_ReturnTrue()
        {
            var anotherSuccessResult = new Result(ResultValues.Success);

            Assert.IsTrue(Result.Success.Equals(anotherSuccessResult));
        }

        [Test]
        public void Equals_DifRefDifValue_ReturnFalse()
        {
            Assert.IsFalse(Result.Success.Equals(Result.AccessDenied));
        }

        [Test]
        public void GetHashCode_Always_ReturnValue()
        {
            var expected = Result.Success.Code.GetHashCode();
            var actual = Result.Success.GetHashCode();
            Assert.AreEqual(expected, actual);
        }
    }
}
