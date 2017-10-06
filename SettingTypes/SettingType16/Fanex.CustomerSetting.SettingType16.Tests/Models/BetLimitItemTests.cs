namespace Fanex.CustomerSetting.SettingType16.Tests.Models
{
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Dtos;
    using NUnit.Framework;
    using SettingType16.Models;

    public class BetLimitItemTests
    {
        [SuppressMessage(
            "StyleCop.CSharp.StylecopPlus.Rules",
            "SP2102:PropertyMustNotContainMoreLinesThan",
            Justification = "Reviewed.")]
        private static IEnumerable SampleBetLimitForValidateMethod
        {
            get
            {
                yield return new TestCaseData(
                    new BetLimitItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        MinBet = 10,
                        MaxBet = 0,
                        MaxPerMatch = 120
                    }).Returns(7001);
                yield return new TestCaseData(
                    new BetLimitItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        MinBet = 4,
                        MaxBet = 10,
                        MaxPerMatch = 7
                    }).Returns(7002);
                yield return new TestCaseData(
                    new BetLimitItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        MinBet = -1,
                        MaxBet = 2,
                        MaxPerMatch = 3
                    }).Returns(7003);
                yield return new TestCaseData(
                    new BetLimitItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        MinBet = 1,
                        MaxBet = -2,
                        MaxPerMatch = 3
                    }).Returns(7004);
                yield return new TestCaseData(
                    new BetLimitItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        MinBet = 1,
                        MaxBet = 2,
                        MaxPerMatch = -3
                    }).Returns(7005);
                yield return new TestCaseData(
                    new BetLimitItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        MinBet = 1,
                        MaxBet = 2,
                        MaxPerMatch = 100000000000
                    }).Returns(7006);
                yield return new TestCaseData(
                    new BetLimitItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        MinBet = 1,
                        MaxBet = 100000000000,
                        MaxPerMatch = 3
                    }).Returns(7006);
                yield return new TestCaseData(
                    new BetLimitItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        MinBet = 100000000000,
                        MaxBet = 2,
                        MaxPerMatch = 3
                    }).Returns(7006);
                yield return new TestCaseData(
                    new BetLimitItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        MinBet = 1,
                        MaxBet = 2,
                        MaxPerMatch = 3
                    }).Returns(0);
                yield return new TestCaseData(
                    new BetLimitItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        MinBet = 0,
                        MaxBet = 0,
                        MaxPerMatch = 0
                    }).Returns(0);
            }
        }

        private static IEnumerable SampleBetLimitForToXmlMethod
        {
            get
            {
                yield return new TestCaseData(
                    new BetLimitItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        MinBet = 1,
                        MaxBet = 2,
                        MaxPerMatch = 3
                    }).Returns("<BetLimit SportType=\"1\" BetType=\"1\" MinBet=\"1\" MaxBet=\"2\" MaxPerMatch=\"3\"/>");
                yield return new TestCaseData(
                    new BetLimitItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        MinBet = 0,
                        MaxBet = 0,
                        MaxPerMatch = 0
                    }).Returns("<BetLimit SportType=\"1\" BetType=\"1\" MinBet=\"0\" MaxBet=\"0\" MaxPerMatch=\"0\"/>");
            }
        }

        [Test, TestCaseSource(nameof(SampleBetLimitForValidateMethod))]
        public int ValidateMethodReturnsExpectedErrorCode(BetLimitItem betLimit)
        {
            var actualResult = betLimit.Validate();

            return actualResult.Code;
        }

        [Test, TestCaseSource(nameof(SampleBetLimitForToXmlMethod))]
        public string ToXmlMethodReturnsExpectedXmlString(BetLimitItem betLimit)
            => betLimit.ToXml();

        [Test]
        public void Constructor_PopulatesDataFromBetLimitDto()
        {
            // Arrange.
            var betLimitDto = new BetLimitDto
            {
                SportTypeId = 1,
                BetTypeId = 1,
                MinBet = 1,
                MaxBet = 2,
                MaxPerMatch = 3,
                MinBetMin = 0,
                MaxBetMax = 3,
                MaxPerMatchMax = 4
            };

            // Act.
            var actualResult = new BetLimitItem(betLimitDto);

            // Assert.
            var expectedResult = new BetLimitItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                MinBet = 1,
                MaxBet = 2,
                MaxPerMatch = 3,
                Upline = new BetLimitItem
                {
                    MinBet = 0,
                    MaxBet = 3,
                    MaxPerMatch = 4
                }
            };

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}