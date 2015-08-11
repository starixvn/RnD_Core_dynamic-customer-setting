namespace Fanex.CustomerSetting.WebAPI.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.WebAPI.Models.Request;

    public class SettingContext : ISettingContext
    {
        public SettingContext()
        {
        }

        internal SettingContext(SettingRequest request)
        {
            var actor = new User
            {
                Id = request.ActorId,
                Name = request.ActorName,
                Level = Enumeration.FromValue<Level>(request.ActorLevel),
                SubAccountId = request.SubAccountId
            };

            this.Actor = actor;
            this.Language = request.Language;
            this.IP = request.ClientIp;
            this.System = request.System;
            this.Domain = request.Domain;
        }

        public User Actor
        {
            get;
            set;
        }

        public string Language
        {
            get;
            set;
        }

        public string IP
        {
            get;
            set;
        }

        public byte System
        {
            get;
            set;
        }

        public string Domain
        {
            get;
            set;
        }
    }
}