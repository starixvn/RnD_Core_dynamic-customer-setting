namespace Fanex.CustomerSetting.Tests.Dtos.Results
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Results;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ResultTests
    {
        [Test]
        public void IsSuccessful_SubResultsHasData_ExecuteIsSuccessfulFromSubResults()
        {
            //// Arrange
            IResult subResult = Substitute.For<IResult>();
            var result = new Result(new List<IResult>
            {
                subResult
            });

            //// Act
            var isSuccessful = result.IsSuccessful;

            //// Assert
            var checkResult = subResult.Received().IsSuccessful;
        }

        [Test]
        public void IsSuccessful_SubResultsIsNull_DoNotExecuteIsSuccessfulFromSubResults()
        {
            //// Arrange
            IResult subResult = Substitute.For<IResult>();
            var result = new Result(0, string.Empty);

            //// Act
            var isSuccessful = result.IsSuccessful;

            //// Assert
            var checkResult = subResult.DidNotReceive().IsSuccessful;
        }
    }
}