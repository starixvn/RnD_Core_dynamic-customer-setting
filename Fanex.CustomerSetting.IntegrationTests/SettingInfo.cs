namespace Fanex.CustomerSetting.IntegrationTests
{
    using System.Collections.Generic;

    public static class SettingInfo
    {
        public const int CompanyId = 8;
        public const int SuperId = 14424987;
        public const string SuperUserName = "supertest90";
        public const int MasterId = 14425209;
        public const string MasterUserName = "supertest9001";
        public const int AgentId = 14425546;
        public const string AgentUserName = "supertest900101";
        public const int MemberId = 14425599;
        public const string MemberUserName = "supertest900101001";
        public const string Language = "en-US";
        public const string IP = "192.168.1.1";
        public const int SystemId = 1;
        public const string Domain = "www.demo.com";

        public static readonly IEnumerable<int> MasterIds = new List<int> { MasterId, 14763802, 14965577 };
        public static readonly IEnumerable<string> MasterNames = new List<string> { MasterUserName, "SUPERTEST9000", "supertest9001Raf" };
        public static readonly IEnumerable<int> AgentIds = new List<int> { AgentId, 14898339, 14966320 };
        public static readonly IEnumerable<string> AgentNames = new List<string> { AgentUserName, "superrafagent", "supertest9001sunRaf" };
        public static readonly IEnumerable<int> MemberIds = new List<int> { MemberId, 14569862 };
        public static readonly IEnumerable<string> MemberNames = new List<string> { MemberUserName, "supertest900101002" };
    }
}