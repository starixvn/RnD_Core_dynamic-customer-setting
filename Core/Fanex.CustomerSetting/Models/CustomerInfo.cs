using Fanex.CustomerSetting.Configurations;
using System.Linq;

namespace Fanex.CustomerSetting.Models
{
    public class CustomerInfo
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public byte Currency { get; set; }

        public string Site { get; set; }
       
        public string SiteLabel { get; set; }

        public bool IsAllowUncheckStakeLevelOnMember
            => AppSettings.SitesAllowUncheckStakeLevelOnMember.Any(site => site.Equals(this.Site, System.StringComparison.CurrentCultureIgnoreCase));

        public bool IsSupportPT3DecimalSites 
            => AppSettings.SupportPT3DecimalSites.Any(site => site.Equals(this.SiteLabel, System.StringComparison.CurrentCultureIgnoreCase));

        public bool IsY7Sites
            => AppSettings.Y7Sites.Any(site => site.Equals(this.SiteLabel, System.StringComparison.CurrentCultureIgnoreCase));

        public bool IsEnableCollapseExpand { get; set; }

        public bool IsCollapsed { get; set; }
    }
}