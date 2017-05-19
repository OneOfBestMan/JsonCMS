using JsonCMS.Models.PageModels;
using JsonCMS.Models.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Repos
{
    public abstract class RepoBase
    {
        public static RepoBase RepoFactory(string site, dbContext context)
        {

            switch (site)
            {
                case "200Towns":
                    return new JsonCMS.Repos._200Towns.Repo(context);
                case "Top100":
                    return new JsonCMS.Repos._Top100.Repo(context);
            }
            throw new Exception("repo not defined");
        }

        public abstract Pages GetPageSummaryFromDb(bool loadRandomImageNames = false);

        public abstract HtmlRegion GetHtmlFromDb(RegionBase region, string pageName);

        public abstract GalleryRegion GetGalleryFromDb(RegionBase region, string pageName);
    }
}
