using JsonCMS.Models.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Core
{
    public class Domain
    {
        public string domainName { get; set; }
        public bool noSpider { get; set; } = false;
    }

    public class Site
    {
        public string siteTag { get; set; }

        public string viewFolder { get; set; }

        public string siteTitle { get; set; }

        public string subTitle { get; set; }

        public string copyright { get; set; }

        public string analyticsCode { get; set; }

        public List<Domain> domains { get; set; }
    }

    public class Sites
    {
        public List<Site> sites { get; set; }

        public string currentSite { get; set; }

        public Site thisSite { get; set; }

        public void LoadSites (string rootPath)
        {
            var sitesJson = new Json<Sites>(rootPath);
            var sites = sitesJson.ReadJsonObject(sitesJson.ReadFile(".", "sites.json"));
            this.sites = sites.sites;
            this.currentSite = sites.currentSite;
        }
    }
}
