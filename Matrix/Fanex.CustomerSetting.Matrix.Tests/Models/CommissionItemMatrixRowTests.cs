namespace Fanex.CustomerSetting.Matrix.Tests.Models
{
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class CommissionItemMatrixRowTests
    {
        [Test]
        public void CommissionItemMatrixRow_ParameterlessConstructor_AlwaysSuccess()
        {
            var sut = new CommissionItemMatrixRow();
            Assert.NotNull(sut);
        }

        [Test]
        public void CommissionItemMatrixRow_InputNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CommissionItemMatrixRow(null));
        }

        [Test]
        public void CommissionItemMatrixRow_ValidInput_Success()
        {
            var dto = new CommissionItemMatrixDto {
                LevelValue = Level.Super.Value,
                CommRate = 1.01M
            };

            var sut = new CommissionItemMatrixRow(dto);

            Assert.AreEqual(Level.Super, sut.CustomerLevel);
            Assert.AreEqual(1.01M, sut.CommRate);
        }
    }
}
