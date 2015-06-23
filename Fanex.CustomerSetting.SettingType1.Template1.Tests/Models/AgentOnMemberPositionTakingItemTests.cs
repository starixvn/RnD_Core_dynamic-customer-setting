namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class AgentOnMemberPositionTakingItemTests
    {
        private AgentOnMemberPositionTakingItem _agentOnMemberPositionTakingItem;

        [SetUp]
        public void Setup()
        {
            _agentOnMemberPositionTakingItem = new AgentOnMemberPositionTakingItem
            {
                BetTypeId = 1,
                SportTypeId = 1
            };
        }
    }
}
