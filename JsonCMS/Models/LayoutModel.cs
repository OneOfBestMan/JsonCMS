﻿using JsonCMS.Models.Core;
using JsonCMS.Models.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models
{
    public class LayoutModel
    {
        public string PageTitle { get; set; } // for <title> tag
        public string PageDescription { get; set; } // for <description meta tag> 
        public string PageTag { get; set; } // page title in json
        public string PageType { get; set; }
        public string SiteTitle { get; set; }
        public string SiteTag { get; set; }
        public string SubTitle { get; set; }
        public string Copyright { get; set; }
        public string AnalyticsCode { get; set; }
        public string RobotsMeta { get; set; } = string.Empty;

        public LayoutModel(Site site, Page page, Domain domain)
        {
            if (string.IsNullOrEmpty(page.metaTitle))
            {
                this.PageTitle = page.displayName + " — " + site.siteTitle;
            }
            else
            {
                this.PageTitle = page.metaTitle + " — " + site.siteTitle;
            }
            this.PageDescription = page.metaDescription;
            this.PageTag = page.title;
            this.PageType = page.pageType.ToString();
            this.SiteTitle = site.siteTitle;
            this.SubTitle = site.subTitle;
            this.Copyright = site.copyright;
            this.SiteTag = site.siteTag;
            this.AnalyticsCode = site.analyticsCode;
            if (domain != null)
            {
                if (domain.noSpider)
                {
                    RobotsMeta = "<meta name='ROBOTS' content='NOINDEX, NOFOLLOW'>";
                }
            }
        }
    }
}
