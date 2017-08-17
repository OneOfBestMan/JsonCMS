using JsonCMS.Models.Core;
using JsonCMS.Models.Galleries;
using JsonCMS.Models.PageModels;
using JsonCMS.Models.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Repos._GoByFerry
{
    public class Repo : RepoBase
    {
        private dbContext context = null;
        private int randomImageFilenamesToLoad = 17;
        private int maxImagestoCheck = 100;
        private string site = "GoByFerry";
        private string _rootpath;

        private RepoPage pageCache = null;

        public Repo(dbContext context, string rootpath)
        {
            this.context = context;
            this._rootpath = rootpath;
        }

        public override Pages GetPageSummaryFromDb(bool loadRandomImageNames = false)
        {
            Pages pages = new Pages(context);
            pages.pages = new List<Page>();

            var locations = from places in context.ChosenFerrydest
                            orderby places.Location ascending
                            select places;

            foreach (var dbPage in locations)
            {
                var page = new Page(context);

                // fields currently specific to legacy table fields
                page.displayName = dbPage.Location; 
                page.friendlyUrl = "/" + dbPage.Location;
                page.title = dbPage.Location.ToLower();
                page.twitterSearch = dbPage.Twitteralternatives;

                Location location = new Location();
                location.latitude = dbPage.Latitude;
                location.longitude = dbPage.Longitude;
                location.zoom = dbPage.Googlemapzoom ?? 7;
                page.location = location;

                page.grouping = dbPage.Type;
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
                var locations = from places in context.ChosenFerrydest
                                where places.Location == pageName
                                select places;
                pageCache = locations.FirstOrDefault();
            }

            var data = string.Empty;

            if (pageCache != null)
            {
                string wikiPurl = "http://en.wikipedia.org/wiki/" + pageName;
                switch (region.mappedObject)
                {
                    // mapping from objects to database fields
                    case "description":
                        string blurb = ("" + pageCache.Cleaneddatax);
                        var selectPos = blurb.IndexOf("<select");
                        var routes = string.Empty;
                        if (selectPos > 0)
                        {
                            routes = blurb.Substring(selectPos);
                            blurb = blurb.Substring(0, selectPos);
                        }
                        data = blurb;
                        break;
                }
            }

            htmlRegion.data = new Html(data);
            return htmlRegion;
        }

        public override GalleryRegion GetGalleryFromDb(RegionBase region, string pageName, string rootPath)
        {
            Gallery g = new Gallery();
            GalleryRegion galleryRegion = new GalleryRegion(region);
            switch (region.mappedObject) {
                case "thumbImages": g = GetThumbGallery(pageName, rootPath); break;
            }

            galleryRegion.data = g;
            return galleryRegion;
        }

        private Gallery GetThumbGallery(string location, string rootPath)
        {
            int thumbnailSize = 280;

            int mainImageMaxWidth = 2000; // note: images aren't really this size more like 640x480
            int mainImageMaxHeight = 860;

            Gallery g = new Gallery();
            g.imageData = new List<ImageData>();

            // build list of images
            var thumbs = GetThumbs(location);
            g.imageData.AddRange(thumbs);

            // add to gallery structure                

            foreach (var image in g.imageData)
            {

                // thumbnail
                ImageSize thumbSize = new ImageSize(thumbnailSize, thumbnailSize, CropType.Square);
                ImagePath thumbPath = new ImagePath(rootPath, site + "/nails", "thumb_4t" + thumbnailSize + "_" + image.imageName);
                ImageVersion thumbImage = new ImageVersion(ImageVersionTypes.DesktopForGallery, thumbSize, thumbPath);
                image.Versions.Add(thumbImage);

                // large popup
                ImageSize desktopForGalleryImageSize = new ImageSize(mainImageMaxHeight, mainImageMaxWidth, CropType.None);
                ImagePath desktopForGalleryImagePaths = new ImagePath(rootPath, site + "/nails", "thumb_b" + mainImageMaxHeight + "_" + mainImageMaxWidth + "_" + image.imageName);
                ImageVersion desktopForGalleryImage = new ImageVersion(ImageVersionTypes.DesktopMaxSize, desktopForGalleryImageSize, desktopForGalleryImagePaths);
                image.Versions.Add(desktopForGalleryImage);
            }
            return g;
        }

        private List<ImageData> GetThumbs(string location)
        {
            Random random = new Random();
            List<ImageData> thumbs = new List<ImageData>();

            // clean this up
            var thumbImgs = from twn in context.ChosenImagesFromFerrydest
                            where (twn.Location == location)
                    orderby twn.Location
                    select twn;

            var result = thumbImgs.OrderBy(elem => Guid.NewGuid());

            int c = 0;
            foreach (var nthItem in result)
            {
                var thumb = "thumb_4t280_" + nthItem.Imagefilename;
                if (File.Exists(_rootpath + "/GoByFerry/nails/" + thumb))
                {
                    c++;
                    ImageData image = new ImageData();
                    image.imageName = nthItem.Imagefilename;
                    image.url = "http://www.flickr.com/photos/" + nthItem.Ownerid;
                    image.author = nthItem.Ownername;
                    image.alt = nthItem.Title;
                    image.title = nthItem.Title;
                    image.fileExists = true;
                    image.height = nthItem.Largestimageheight;
                    image.width = nthItem.Largestimagewidth;
                    image.description = nthItem.Description;
                    thumbs.Add(image);
                }
                if (c == 4) {
                    break; 
                }
            }

            return thumbs;
        } 

        private void LoadRandomImageNames(Pages pages)
        {
            var images = (from i in context.ChosenImagesFromFerrydest
                          where (
                              (i.Largestimageheight > 750) &&
                              (i.Largestimagewidth > 750) &&
                              (i.Largestimageheight < 1050) &&
                              (i.Largestimagewidth < 1050))
                          select new
                          {
                              i.Location,
                              i.Imagefilename
                          }).OrderBy(y => Guid.NewGuid()).Take(maxImagestoCheck).ToList();

            Dictionary<string, string> randomImages = new Dictionary<string, string>();
            int leftToFind = randomImageFilenamesToLoad;
            int index = 0;
            while (leftToFind > 0 && index < maxImagestoCheck)
            {
                var image = images.Skip(index).Take(1).First();
                var thumb = "thumb_4t280_" + image.Imagefilename;
                if (File.Exists(_rootpath + "/GoByFerry/nails/" + thumb)) {
                    if (!randomImages.Keys.Contains(image.Location.ToLower()))
                    {
                        randomImages.Add(image.Location.ToLower(), image.Imagefilename);
                        leftToFind--;
                    }
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
