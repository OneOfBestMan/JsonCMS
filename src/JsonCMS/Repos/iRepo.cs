using JsonCMS.Models.Galleries;
using JsonCMS.Models.PageModels;
using JsonCMS.Models.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Repos
{
    public interface iRepo
    {
        Pages GetPageSummaryFromDb(bool loadRandomImageNames = false);

        HtmlRegion GetHtmlFromDb(RegionBase region, string pageName);

        GalleryRegion GetGalleryFromDb(RegionBase region, string pageName);
    }
}
