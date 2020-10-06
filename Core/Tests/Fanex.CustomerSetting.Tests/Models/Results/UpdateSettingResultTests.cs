namespace Fanex.CustomerSetting.Tests.Models.Results
{
    using Fanex.CustomerSetting.Models.Constants;
    using Fanex.CustomerSetting.Models.Results;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    [TestFixture]
    public class UpdateSettingResultTests
    {
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

        [Test]
        public void UpdateSettingResult_InitWithMultipleTypeOfIResult_ReturnExpectedResult()
        {
            var results = new List<IResult>();
            results.Add(new UpdateSettingResult(Result.Success));
            results.Add(Result.Success);

            var actual = new UpdateSettingResult(results);

            var expected = new List<SettingResult>();
            expected.AddRange((new UpdateSettingResult(Result.Success)).SubResults);
            expected.Add(new SettingResult(Result.Success));

            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual.SubResults));
        }

        [Test]
        public void RemoveNoChangeByUMSubResults_SubResultsWithUMSubResult_UMSubResultWillBeRemoved()
        {
            var expectedResults = new List<IResult>();
            expectedResults.Add(Result.Success);
            var expected = new UpdateSettingResult(expectedResults);

            var actualResults = new List<IResult>();
            actualResults.Add(Result.Success);
            actualResults.Add(new Result(ResultValues.SuccessButDoNothingBecauseUM));
            var actual = new UpdateSettingResult(actualResults);
            actual.RemoveNoChangeByUMSubResults();

            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual));
        }

        [Test]
        public void IsSuccessful_NoSubResults_ReturnExpectedResult()
        {
            var subResults = new List<IResult>();
            var actual = new UpdateSettingResult(subResults);
            Assert.IsTrue(actual.IsSuccessful);
        }
    }
}
