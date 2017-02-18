using JsonCMS.Models.Core;
using JsonCMS.Models.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models
{

    public class JsonData
    {

        public Sites sites = null;
        public SocialNetworks social = null;
        public Pages pages = null;
        public Site currentSite;
        public Page thisPage = null;
        public Menus menus = null;
        public string currentRegion = null;
        public string currentHost = null;
        public Domain currentDomain = null;

        public void LoadJsonForPage(string pageName, string rootPath, string parameter = "", string siteTag = "")
        {
            LoadGlobalJson(rootPath, siteTag);

            if (!string.IsNullOrEmpty(pageName) && pages!=null && pages.pages!=null)
            {
                thisPage = pages.pages.Where(x => x.title == pageName.ToLower()).FirstOrDefault();
                if (thisPage != null)
                {
                    if (currentSite != null)
                    {
                        thisPage.LoadPageData(rootPath, currentSite.siteTag, parameter);
                    }
                }
            }
       }

        public void LoadGlobalJson(string rootPath, string siteTag)
        {
            sites = new Sites();
            sites.LoadSites(rootPath);

            GetCurrentSite(siteTag);

            if (currentSite!=null)
            {
                social = new SocialNetworks();
                social.LoadSocialNetworks(rootPath, currentSite.siteTag);

                pages = new Pages();
                pages.LoadPages(rootPath, currentSite.siteTag);

                menus = new Menus();
                menus.LoadMenus(rootPath, pages, currentSite.siteTag);
            }
        }

        private void GetCurrentSite(string siteTag)
        {
            if (siteTag != null)
            {
                sites.thisSite = sites.sites.Where(x => x.siteTag == siteTag).FirstOrDefault();
            }
            else
            {
                var matchingDomains = sites.sites.Where(x => x.domains.Any(y => y.domainName == this.currentHost));

                if (matchingDomains.Count() > 0)
                {
                    sites.thisSite = matchingDomains.FirstOrDefault();

                }
                else
                {
                    sites.thisSite = sites.sites.Where(x => x.siteTag == sites.currentSite).FirstOrDefault();
                }
            }

            if (sites.thisSite!=null)
            {
                currentDomain = sites.thisSite.domains.Where(y => y.domainName == this.currentHost).FirstOrDefault();
            }
            currentSite = sites.thisSite;
        }
    }
}
