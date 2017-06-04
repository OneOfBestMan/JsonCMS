using JsonCMS.Models.Core;
using JsonCMS.Models.Galleries;
using JsonCMS.Models.PageModels;
using JsonCMS.Models.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Repos._200Towns
{
    public class Repo : RepoBase
    {
        private dbContext context = null;
        private int randomImageFilenamesToLoad = 17;
        private int maxImagestoCheck = 100;
        private string site = "200Towns";

        private RepoPage pageCache = null;

        public Repo(dbContext context)
        {
            this.context = context;
        }

        public override Pages GetPageSummaryFromDb(bool loadRandomImageNames = false)
        {
            Pages pages = new Pages(context);
            pages.pages = new List<Page>();

            var locations = from towns in context.ChosenTowns
                            orderby towns.town ascending
                            select towns;

            foreach (var dbPage in locations)
            {
                var page = new Page(context);

                // fields currently specific to legacy table fields
                page.displayName = dbPage.town; 
                page.friendlyUrl = "/" + dbPage.town;
                page.title = dbPage.town.ToLower();
                page.twitterSearch = dbPage.town; 

                Location location = new Location();
                location.latitude = dbPage.latitude;
                location.longitude = dbPage.longitude;
                location.zoom = 9;
                page.location = location;

                page.grouping = dbPage.grouping;
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
                var locations = from towns in context.ChosenTowns
                                where towns.town == pageName
                                select towns;
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

        public override GalleryRegion GetGalleryFromDb(RegionBase region, string pageName, string rootPath)
        {
            Gallery g = new Gallery();
            GalleryRegion galleryRegion = new GalleryRegion(region);
            switch (region.mappedObject)
            {
                case "mainImage": g = GetMainGallery(pageName, rootPath); break;
                case "thumbImages": g = GetThumbGallery(pageName, rootPath); break;
            }

            galleryRegion.data = g;
            return galleryRegion;
        }

        private Gallery GetMainGallery(string location, string rootPath)
        {
            int mainImageMaxWidth = 2000; // note: images aren't really this size more like 640x480
            int mainImageMaxHeight = 860;

            Gallery g = new Gallery();
            g.imageData = new List<ImageData>();

            // build list of images
            var main = GetMainImage(location);
            g.imageData.Add(main);

            // add to gallery structure                

            foreach (var image in g.imageData)
            {
                // main image
                ImageSize desktopForGalleryImageSize = new ImageSize(mainImageMaxHeight, mainImageMaxWidth, CropType.None);
                ImagePath desktopForGalleryImagePaths = new ImagePath(rootPath, site + "/nails", "thumb_b" + mainImageMaxHeight + "_" + mainImageMaxWidth + "_" + image.imageName);
                ImageVersion desktopForGalleryImage = new ImageVersion(ImageVersionTypes.DesktopMaxSize, desktopForGalleryImageSize, desktopForGalleryImagePaths);
                image.Versions.Add(desktopForGalleryImage);
            }
            return g;
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
            var thumbImgs = from twn in context.chosenImagesFromTowns
                            where (twn.searchtown == location)
                            orderby twn.searchtown
                            select twn;

            var result = thumbImgs.OrderBy(elem => Guid.NewGuid());

            foreach (var nthItem in result.Take(7)) // one more than required as may include main image
            {
                ImageData image = new ImageData();
                image.imageName = nthItem.imagefilename;
                image.url = "http://www.flickr.com/photos/" + nthItem.ownerid;
                image.author = nthItem.ownername;
                image.alt = nthItem.title;
                image.title = nthItem.title;
                image.fileExists = true;
                image.height = nthItem.largestimageheight;
                image.width = nthItem.largestimagewidth;
                image.description = nthItem.description;
                thumbs.Add(image);
            }

            return thumbs;
        }

        private ImageData GetMainImage(string location)
        {
            Random random = new Random();

            // clean this up
            var images = from twn in context.chosenImagesFromTowns
                         where (twn.largestimagewidth > twn.largestimageheight)
                         && (twn.largestimagewidth >= 840) && (twn.searchtown == location)
                         orderby twn.searchtown
                         select twn;

            int randomNumber = random.Next(0, images.Count()) + 1;
            RepoGallery nthItem;

            try
            {
                nthItem = images.Skip(randomNumber).First();
            }
            catch (Exception e)
            { // if cant find large image use smaller
                var q2 = from twn in context.chosenImagesFromTowns
                         where (twn.largestimagewidth > twn.largestimageheight)
                         && (twn.largestimagewidth >= 600) && (twn.searchtown == location)
                         orderby twn.searchtown
                         select twn;
                nthItem = q2.First();
            }

            ImageData image = new ImageData();
            image.imageName = nthItem.imagefilename;
            image.url = "http://www.flickr.com/photos/" + nthItem.ownerid;
            image.author = nthItem.ownername;
            image.alt = nthItem.title;
            image.title = nthItem.title;
            image.fileExists = true;
            image.height = nthItem.largestimageheight;
            image.width = nthItem.largestimagewidth;
            image.description = nthItem.description;

            return image;
        }

        private void LoadRandomImageNames(Pages pages)
        {
            var images = (from i in context.chosenImagesFromTowns
                          where (
                              (i.largestimageheight > 750) &&
                              (i.largestimagewidth > 750) &&
                              (i.largestimageheight < 1050) &&
                              (i.largestimagewidth < 1050))
                          select new
                          {
                              i.searchtown,
                              i.imagefilename
                          }).OrderBy(y => Guid.NewGuid()).Take(maxImagestoCheck).ToList();

            Dictionary<string, string> randomImages = new Dictionary<string, string>();
            int leftToFind = randomImageFilenamesToLoad;
            int index = 0;
            while (leftToFind > 0)
            {
                var image = images.Skip(index).Take(1).First();
                if (!randomImages.Keys.Contains(image.searchtown.ToLower()))
                {
                    randomImages.Add(image.searchtown.ToLower(), image.imagefilename);
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
