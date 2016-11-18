using JsonCMS.Models.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonCMS.Models.Regions;

namespace JsonCMS.Models.PageModels
{
    public class Pages
    {
        public List<Page> pages { get; set; }

        public void LoadPages(string rootPath, string site)
        {
            var pagesJson = new Json<Pages>(rootPath);
            var pages = pagesJson.ReadJsonObject(pagesJson.ReadFile(site + "/CMSdata/pages", "pages.json"));
            foreach (var page in pages.pages)
            {
                int sequence = 0;
                foreach (var region in page.regions)
                {
                    sequence++;
                    region.sequence = sequence;                   
                }
            }
            this.pages = pages.pages;
        }
    }


}
