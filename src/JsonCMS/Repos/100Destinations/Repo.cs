using JsonCMS.Models.Core;
using JsonCMS.Models.Galleries;
using JsonCMS.Models.PageModels;
using JsonCMS.Models.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Repos._Top100
{
    public class Repo : RepoBase
    {
        private dbContext context = null;
        private int randomImageFilenamesToLoad = 17;
        private int maxImagestoCheck = 100;

        private RepoPage pageCache = null;

        public Repo(dbContext context)
        {
            this.context = context;
        }

        public override Pages GetPageSummaryFromDb(bool loadRandomImageNames = false)
        {
            Pages pages = new Pages(context);
            pages.pages = new List<Page>();

            var locations = from places in context.Chosen100dest
                            orderby places.location ascending
                            select places;

            foreach (var dbPage in locations)
            {
                var page = new Page(context);

                // fields currently specific to legacy table fields
                page.displayName = dbPage.location; 
                page.friendlyUrl = "/" + dbPage.location;
                page.title = dbPage.location.ToLower();

                Location location = new Location();

                float latitude;
                if (float.TryParse(dbPage.latitude, out latitude))
                {
                    location.latitude = latitude;
                }
                float longitude;
                if (float.TryParse(dbPage.longitude, out longitude))
                {
                    location.longitude = longitude;
                }
                location.zoom = dbPage.googlemapzoom ?? 7;
                page.location = location;

                page.grouping = dbPage.continent;
                page.source = Source.Db;
                pages.pages.Add(page);
            } 

            if (loadRandomImageNames)
            {
                LoadRandomImageNames(pages);
            }

            return pages;
        }

        public override HtmlRegion GetHtmlFromDb(RegionBase region, string pageName)
        {
            HtmlRegion htmlRegion = new HtmlRegion(region);

            if (pageCache == null)
            {
                var locations = from places in context.Chosen100dest
                                where places.location == pageName
                                select places;
                pageCache = locations.FirstOrDefault();
            }

            var data = string.Empty;

            if (pageCache != null)
            {
                switch (region.mappedObject)
                {
                    // mapping from objects to database fields
                    case "description": data = pageCache.cleaneddata; break;
                }
            }

            htmlRegion.data = new Html(data);
            return htmlRegion;
        }

        public override GalleryRegion GetGalleryFromDb(RegionBase region, string pageName)
        {
            GalleryRegion galleryRegion = new GalleryRegion(region);
            return galleryRegion;
        }

        private void LoadRandomImageNames(Pages pages)
        {
            var images = (from i in context.chosenImagesFrom100Dest
                          where (
                              (i.largestimageheight > 750) &&
                              (i.largestimagewidth > 750) &&
                              (i.largestimageheight < 1050) &&
                              (i.largestimagewidth < 1050))
                          select new
                          {
                              i.location,
                              i.imagefilename
                          }).OrderBy(y => Guid.NewGuid()).Take(maxImagestoCheck).ToList();

            Dictionary<string, string> randomImages = new Dictionary<string, string>();
            int leftToFind = randomImageFilenamesToLoad;
            int index = 0;
            while (leftToFind > 0)
            {
                var image = images.Skip(index).Take(1).First();
                if (!randomImages.Keys.Contains(image.location.ToLower()))
                {
                    randomImages.Add(image.location.ToLower(), image.imagefilename);
                    leftToFind--;
                }
                index++;
            }
            foreach (var image in randomImages)
            {
                pages.pages.Where(x => x.displayName.ToLower() == image.Key.ToLower()).First().exampleImage = image.Value;
            }
        }

    }
}
