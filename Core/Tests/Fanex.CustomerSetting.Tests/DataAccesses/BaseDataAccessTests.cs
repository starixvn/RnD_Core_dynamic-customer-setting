namespace Fanex.CustomerSetting.Tests.DataAccesses
{
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections;
    using System.Web.Script.Serialization;

    public class MockBaseDataAccess : BaseDataAccess
    {
        public new IResult ExecuteNonQuery(string settingKey, Hashtable storedProcedureParam)
            => base.ExecuteNonQuery(settingKey, storedProcedureParam);

        public new IResult GetResult(Hashtable param)
            => base.GetResult(param);

        public new Hashtable GetSPParamsForGettingSettingType(CustomerSettingQuery querySettingParam, byte settingType)
            => base.GetSPParamsForGettingSettingType(querySettingParam, settingType);

    }

    [TestFixture]
    public class BaseDataAccessTests
    {
        private MockBaseDataAccess mockBaseDataAccess;

        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

        [SetUp]
        public void SetUp()
        {
            mockBaseDataAccess = new MockBaseDataAccess();
            var subObjectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.ObjectDb = (settingKey) => subObjectDb;
        }

        [Test]
        public void ExecuteNonQuery_Always_Success()
        {
            Assert.DoesNotThrow(() => mockBaseDataAccess.ExecuteNonQuery(null, GetSpParams()));
        }

        [Test]
        public void GetResult_ParamIsNull_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => mockBaseDataAccess.GetResult(null));
        }

        [Test]
        public void GetResult_ParamIsMissingErrCodeErrMsg_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => mockBaseDataAccess.GetResult(new Hashtable {
                { "param1", 0},
                { "param2", ""}
            }));
        }

        [Test]
        public void GetResult_ParamIsValid_ReturnExpectedResult()
        {
            var expected = new Result(0);
            var actual = mockBaseDataAccess.GetResult(GetSpParams());

            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual));
        }

        [Test]
        public void GetSPParamsForGettingSettingType_QueryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => mockBaseDataAccess.GetSPParamsForGettingSettingType(null, 0));
        }

        [Test]
        public void GetSPParamsForGettingSettingType_MemberLevel_ReturnResultWithRoleIdOfUplineLevel()
        {
            var expected = new Hashtable
            {
                { "custid", 1 },
                { "productid", 1 },
                { "settingtype", 0 },
                { "roleid", 3}
            };

            var actual = mockBaseDataAccess.GetSPParamsForGettingSettingType(new CustomerSettingQuery(1, 1, 1, 3), 0);
            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual));
        }

        private Hashtable GetSpParams()
        {
            return new Hashtable
            {
                { "errcode", 0},
                { "errmsg", ""}
            };
        }

    }
}
