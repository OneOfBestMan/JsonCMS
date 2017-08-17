using JsonCMS.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Regions
{
    public class BlogRegion : RegionBase
    {
        public BlogPage currentPage = null;
        public BlogPage firstPage = null;
        public string friendlyUrl { get; set; }
        public bool lastPage { get; set; }

        public BlogRegion(RegionBase regionBase)
        {
            this.sequence = regionBase.sequence;
            this.templateTag = regionBase.templateTag;
            this.mappedObject = regionBase.mappedObject;
            this.SetType(regionBase.regionType);
            this.title = regionBase.title;
        }

        public void LoadData(string rootPath, string blogName, string site, string parameter, string friendlyUrl)
        {
            this.friendlyUrl = friendlyUrl;

            currentPage = new BlogPage();
            firstPage = new BlogPage();

            int pageNo;
            if(!int.TryParse(parameter, out pageNo)){
                pageNo = 1;
            }

            currentPage.ReadPage(rootPath, blogName, site, pageNo);
            firstPage.ReadPage(rootPath, blogName, site, 1);

            lastPage = pageNo == Math.Ceiling((decimal)currentPage.TotalEntries / currentPage.entriesPerPage);
        }
    }
}
