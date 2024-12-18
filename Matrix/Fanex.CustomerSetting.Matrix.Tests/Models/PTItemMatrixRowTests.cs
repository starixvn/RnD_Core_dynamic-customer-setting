namespace Fanex.CustomerSetting.Matrix.Tests.Models
{
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using NUnit.Framework;
    using System;
    using System.Web.Script.Serialization;

    [TestFixture]
    public class PTItemMatrixRowTests
    {
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

        [Test]
        public void PTItemMatrixRow_ParameterlessConstructor_AlwaysSuccess()
        {
            var sut = new PTItemMatrixRow();
            Assert.NotNull(sut);
        }

        [Test]
        public void PTItemMatrixRow_NullInput_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PTItemMatrixRow(null, DirectMember.None));
        }

        [Test]
        public void PTItemMatrixRow_SuperLevel_ReturnExpectedResult()
        {
            var fakeData = new PTItemMatrixDto {
                LevelValue = Level.Super.Value,
                SuperPT = 1,
                MasterPT = 1,
            };

            var expected = new PTItemMatrixRow {
                CustomerLevel = Level.Super,
                SuperPT = 1,
                MasterPT = 1
            };

            var actual = new PTItemMatrixRow(fakeData, DirectMember.None);

            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual));
        }

        [Test]
        public void PTItemMatrixRow_MasterLevel_IsNotAutoSuperPT_ReturnExpectedResult()
        {
            var fakeData = new PTItemMatrixDto
            {
                LevelValue = Level.Master.Value,
                SuperPT = 1,
                MasterPT = 1,
                IsAutoSuperPT = false
            };

            var expected = new PTItemMatrixRow
            {
                CustomerLevel = Level.Master,
                SuperPT = 1,
                MasterPT = 1,
                IsAutoSuperPT = false,
                AutoSuperPT = 0
            };

            var actual = new PTItemMatrixRow(fakeData, DirectMember.None);

            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual));
        }

        [Test]
        public void PTItemMatrixRow_MasterLevel_IsAutoSuperPT_ReturnExpectedResult()
        {
            var fakeData = new PTItemMatrixDto
            {
                LevelValue = Level.Master.Value,
                SuperPT = 1,
                MasterPT = 1,
                IsAutoSuperPT = true
            };

            var expected = new PTItemMatrixRow
            {
                CustomerLevel = Level.Master,
                SuperPT = 0,
                MasterPT = 1,
                IsAutoSuperPT = true
            };

            var actual = new PTItemMatrixRow(fakeData, DirectMember.None);

            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual));
        }

        [Test]
        public void PTItemMatrixRow_AgentLevel_IsAutoSuperPT_ReturnExpectedResult()
        {
            var fakeData = new PTItemMatrixDto
            {
                LevelValue = Level.Agent.Value,
                MasterPT = 1,
                IsAutoMasterPT = true
            };

            var expected = new PTItemMatrixRow
            {
                CustomerLevel = Level.Agent,
                MasterPT = 0,
                IsAutoMasterPT = true
            };

            var actual = new PTItemMatrixRow(fakeData, DirectMember.None);

            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual));
        }

        [Test]
        public void PTItemMatrixRow_AgentLevel_IsNotAutoSuperPT_ReturnExpectedResult()
        {
            var fakeData = new PTItemMatrixDto
            {
                LevelValue = Level.Agent.Value,
                MasterPT = 1,
                IsAutoMasterPT = false
            };

            var expected = new PTItemMatrixRow
            {
                CustomerLevel = Level.Agent,
                MasterPT = 1,
                IsAutoMasterPT = false,
                AutoMasterPT = 0
            };

            var actual = new PTItemMatrixRow(fakeData, DirectMember.None);

            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual));
        }

        [Test]
        public void PTItemMatrixRow_MemberLevel_ReturnExpectedResult()
        {
            var fakeData = new PTItemMatrixDto
            {
                LevelValue = Level.Member.Value,
                SuperPT = 1,
                MasterPT = 1,
                AgentPT = 1,
                IsAutoSuperPT = true,
                AutoSuperPT = 10,
                IsAutoMasterPT = true,
                AutoMasterPT = 20
            };

            var expected = new PTItemMatrixRow
            {
                CustomerLevel = Level.Member,
                SuperPT = 1,
                MasterPT = 1,
                AgentPT = 1,
                IsAutoSuperPT = true,
                AutoSuperPT = 10,
                IsAutoMasterPT = true,
                AutoMasterPT = 20
            };

            var actual = new PTItemMatrixRow(fakeData, DirectMember.None);

            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual));
        }
    }
}
