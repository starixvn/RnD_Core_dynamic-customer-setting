namespace Fanex.CustomerSetting.SettingType1.Tests.DataAccesses
{
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType1.DataAccesses;
    using Fanex.CustomerSetting.SettingType1.Models;
    using Fanex.CustomerSetting.SettingType1.Resources;
    using Fanex.CustomerSetting.WebApi.Models;
    using Fanex.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Serialization;

    [TestFixture]
    public class DataAccessTests
    {
        private IDataAccess dataAccess;
        private IObjectDb subObjectDb;
        private readonly JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        [SetUp]
        public void SetUp()
        {
            var objectDbSub = Substitute.For<IObjectDb>();
            objectDbSub.Query<SuperPTItem>(Arg.Any<Hashtable>()).Returns(new List<SuperPTItem>() { new SuperPTItem() });
            objectDbSub.Query<MasterPTItem>(Arg.Any<Hashtable>()).Returns(new List<MasterPTItem>() { new MasterPTItem() });
            objectDbSub.Query<AgentPTItem>(Arg.Any<Hashtable>()).Returns(new List<AgentPTItem>() { new AgentPTItem() });
            objectDbSub.Query<SuperPTOnMemberItem>(Arg.Any<Hashtable>()).Returns(new List<SuperPTOnMemberItem>() { new SuperPTOnMemberItem() });
            objectDbSub.Query<MasterPTOnMemberItem>(Arg.Any<Hashtable>()).Returns(new List<MasterPTOnMemberItem>() { new MasterPTOnMemberItem() });
            objectDbSub.Query<AgentPTOnMemberItem>(Arg.Any<Hashtable>()).Returns(new List<AgentPTOnMemberItem>() { new AgentPTOnMemberItem() });
            objectDbSub.Query<int>(Arg.Any<object>()).Returns(new List<int> { 1, 2, 3 });
            objectDbSub.ExecuteNonQuery().Returns(0);
            ObjectDbFactory.ObjectDb = (settingKey) => objectDbSub;
            dataAccess = new DataAccess();
        }

        [Test]
        public void GetPTSettingItem_AnyArgument_ReturnExpectedResult()
        {
            var expectedResult = new List<PTItem>();
            var subObjDb = Substitute.For<IObjectDb>();
            subObjDb.Query<PTItem>(Arg.Any<Hashtable>()).Returns(expectedResult);
            var actualResult = DataAccess.GetPTSettingItem<PTItem>(subObjDb, new Hashtable());
            Assert.AreEqual(expectedResult, actualResult);
        }

        static object[] GetCustomerPTValidQueryTestCases = new object[]
        {
            new object[]
            {
                new CustomerSettingQuery(1, Level.Super, 1),
                new PTSettingType
                {
                    Name = Label.PositionTaking,
                    SettingItems = new List<SuperPTItem>() { new SuperPTItem() }
                }
            },
            new object[]
            {
                new CustomerSettingQuery(1, Level.Master, 1),
                new PTSettingType
                {
                    Name = Label.PositionTaking,
                    SettingItems = new List<MasterPTItem>() { new MasterPTItem() }
                }
            },
            new object[]
            {
                new CustomerSettingQuery(1, Level.Agent, 1),
                new PTSettingType
                {
                    Name = Label.PositionTaking,
                    SettingItems = new List<AgentPTItem>() { new AgentPTItem() }
                }
            },
            new object[]
            {
                new CustomerSettingQuery(1, Level.Member, 1, 0,Level.Super),
                new PTSettingType
                {
                    Name = Label.PositionTaking,
                    SettingItems = new List<SuperPTOnMemberItem>() { new SuperPTOnMemberItem() }
                }
            },
            new object[]
            {
                new CustomerSettingQuery(1, Level.Member, 1, 0, Level.Master),
                new PTSettingType
                {
                    Name = Label.PositionTaking,
                    SettingItems = new List<MasterPTOnMemberItem>() { new MasterPTOnMemberItem() }
                }
            },
            new object[]
            {
                new CustomerSettingQuery(1, Level.Member, 1, 0, Level.Agent),
                new PTSettingType
                {
                    Name = Label.PositionTaking,
                    SettingItems = new List<AgentPTOnMemberItem>() { new AgentPTOnMemberItem() }
                }
            },
        };

        [Test]
        [TestCaseSource("GetCustomerPTValidQueryTestCases")]
        public void GetCustomerPT_ValidQuery_ReturnExpectedResult(CustomerSettingQuery query, PTSettingType expectedResult)
        {
            var actualResult = dataAccess.GetCustomerPT(query);
            Assert.AreEqual(javaScriptSerializer.Serialize(expectedResult), javaScriptSerializer.Serialize(actualResult));
        }

        static object[] GetCustomerPTInvalidQueryTestCases = new object[]
        {
            new object[]
            {
                new CustomerSettingQuery(1, Level.Super, 1),
                new PTSettingType
                {
                    Name = Label.PositionTaking,
                    SettingItems = new List<MasterPTItem>() { new MasterPTItem() }
                }
            },
            new object[]
            {
                new CustomerSettingQuery(1, Level.Master, 1),
                new PTSettingType
                {
                    Name = Label.PositionTaking,
                    SettingItems = new List<AgentPTItem>() { new AgentPTItem() }
                }
            },
            new object[]
            {
                new CustomerSettingQuery(1, Level.Agent, 1),
                new PTSettingType
                {
                    Name = Label.PositionTaking,
                    SettingItems = new List<SuperPTItem>() { new SuperPTItem() }
                }
            },
            new object[]
            {
                new CustomerSettingQuery(1, Level.Member, 1, 0,Level.Super),
                new PTSettingType
                {
                    Name = Label.PositionTaking,
                    SettingItems = new List<MasterPTOnMemberItem>() { new MasterPTOnMemberItem() }
                }
            },
            new object[]
            {
                new CustomerSettingQuery(1, Level.Member, 1, 0, Level.Master),
                new PTSettingType
                {
                    Name = Label.PositionTaking,
                    SettingItems = new List<AgentPTOnMemberItem>() { new AgentPTOnMemberItem() }
                }
            },
            new object[]
            {
                new CustomerSettingQuery(1, Level.Member, 1, 0, Level.Agent),
                new PTSettingType
                {
                    Name = Label.PositionTaking,
                    SettingItems = new List<SuperPTOnMemberItem>() { new SuperPTOnMemberItem() }
                }
            },
        };

        [Test]
        [TestCaseSource("GetCustomerPTInvalidQueryTestCases")]
        public void GetCustomerPT_InvalidQuery_AssertNotEqual(CustomerSettingQuery query, PTSettingType expectedResult)
        {
            var actualResult = dataAccess.GetCustomerPT(query);
            Assert.AreNotEqual(javaScriptSerializer.Serialize(expectedResult), javaScriptSerializer.Serialize(actualResult));
        }

        [Test]
        public void UpdateCustomerPT_ValidInputs_ReturnExpectedResult()
        {
            var settingContext = new SettingContext
            {
                Actor = new User
                {
                    Id = 8,
                    Name = "Test",
                    Level = Level.Company,
                    SubAccountId = 0
                },
                Language = string.Empty,
                IP = string.Empty,
                System = 0,
                Domain = string.Empty
            };

            var actualResult = dataAccess.UpdateCustomerPT(settingContext, 1, Level.Master, Level.Super, string.Empty);
            var expectedResult = new Result(0, string.Empty, 0);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void GetGroupedMemberIds_ValidMemberList_ReturnExpectedResult()
        {
            var expectedResult = new List<int> { 1, 2, 3 };
            var actualResult = dataAccess.GetGroupedMemberIds(string.Empty);
            Assert.AreEqual(expectedResult, actualResult.ToList());
        }

        static object[] UpdateGroupedMembersSettingValidTestCases = new object[]
        {
            new object[] { 0, 0, string.Empty, Level.Super, string.Empty, string.Empty },
            new object[] { 0, 0, string.Empty, Level.Master, string.Empty, string.Empty },
            new object[] { 0, 0, string.Empty, Level.Agent, string.Empty, string.Empty }
        };

        [Test]
        [TestCaseSource("UpdateGroupedMembersSettingValidTestCases")]
        public void UpdateGroupedMembersSetting_ValidInput_ReturnExpectedResult(int actorId,
            int subAccountId,
            string groupedMemberIds,
            Level uplineLevel,
            string positionTakingXml,
            string ip)
        {
            var expectedResult = new Result(0, string.Empty, 0);
            var actualResult = dataAccess.UpdateGroupedMembersSetting(
                actorId,
                subAccountId,
                groupedMemberIds,
                uplineLevel,
                positionTakingXml,
                ip);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
