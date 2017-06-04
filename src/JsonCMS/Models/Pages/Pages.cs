using JsonCMS.Models.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonCMS.Models.Regions;
using JsonCMS.Repos;

namespace JsonCMS.Models.PageModels
{
    public class Pages
    {
        private dbContext context = null;

        public Pages(dbContext context)
        {
            this.context = context;
        }

        public List<Page> pages { get; set; }
        public Templates templates { get; set; }

        public void LoadPages(string rootPath, string site, bool loadPagesFromDb)
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

            if (loadPagesFromDb)
            {
                AddPagesFromDb(site, rootPath);
            }
        }

        private void AddPagesFromDb(string site, string rootPath)
        {
            var pageTemplatesJson = new Json<Templates>(rootPath);
            templates = pageTemplatesJson.ReadJsonObject(pageTemplatesJson.ReadFile(site + "/CMSdata/pages", "pageTemplates.json"));

            RepoBase repo = RepoBase.RepoFactory(site, context);

            if (repo == null)
            {
                throw new Exception("Repo not defined");
            } 
                       
            var repoPages = repo.GetPageSummaryFromDb(true);
            foreach (var repoPage in repoPages.pages)
            {
                this.pages.Add(repoPage);
            }
        }

    }


}
