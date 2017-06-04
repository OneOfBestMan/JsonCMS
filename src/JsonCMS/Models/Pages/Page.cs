using JsonCMS.Models.Core;
using JsonCMS.Models.Regions;
using JsonCMS.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.PageModels
{

    public class Page
    {
        private dbContext context = null;

        public Page(dbContext context)
        {
            this.context = context;
        }

        public string title { get; set; }

        public string displayName { get; set; } // used for menu title
        public string metaTitle { get; set; } = string.Empty; // page meta title if different to menu title
        public string metaDescription { get; set; } = string.Empty;

        public string friendlyUrl { get; set; }

        public string twitterSearch { get; set; }

        private PageType _pageType = PageType.Standard;

        public string grouping { get; set; } = null; // only used for nested menus

        public Location location { get; set; }

        public string exampleImage { get; set; }

        public Source source { get; set; } = Source.Json;  

        public string template
        {
            set
            {
                _pageType = GetPageType((value.ToLower()));
            }

            get
            {
                return _pageType.ToString().ToLower();
            }
        }

        private PageType GetPageType (string pageType)
        {
            PageType __pageType = PageType.Standard;

            switch (pageType.ToLower())
            {
                case "blog": __pageType = PageType.Blog; break;
                case "twocolumn": __pageType = PageType.TwoColumn; break;
                case "standard": __pageType = PageType.Standard; break;
                case "photosection": __pageType = PageType.PhotoSection; break;
                case "carousel": __pageType = PageType.Carousel; break;
                case "miniatures": __pageType = PageType.Miniatures; break;
                case "home": __pageType = PageType.Home; break;
            }
            return __pageType;
        }

        public PageType pageType
        {
            get
            {
                return _pageType;
            }
        }

        public List<RegionBase> regions { get; set; } = new List<RegionBase>();
        public List<GalleryRegion> galleryRegions { get; set; } = new List<GalleryRegion>();
        public List<MapRegion> mapRegions { get; set; } = new List<MapRegion>();
        public List<HtmlRegion> htmlRegions { get; set; } = new List<HtmlRegion>();
        public List<YouTubeRegion> youtubeRegions { get; set; } = new List<YouTubeRegion>();
        public List<BlogRegion> blogRegions { get; set; } = new List<BlogRegion>();

        public void LoadPageData(string rootPath, string site, string parameter)
        {
            foreach (var region in regions)
            {
                switch (region.regionType)
                {
                    case RegionType.Gallery:
                        var galleryRegion = new GalleryRegion(region);
                        galleryRegion.LoadData(rootPath, region.mappedObject, site);
                        galleryRegions.Add(galleryRegion);
                        break;
                    case RegionType.Html:
                        var htmlRegion = new HtmlRegion(region);
                        htmlRegion.LoadData(site, rootPath, title, region.mappedObject);
                        htmlRegions.Add(htmlRegion);
                        break;
                    case RegionType.YouTube:
                        var youtubeRegion = new YouTubeRegion(region);
                        youtubeRegion.LoadData(site, rootPath, title, region.mappedObject);
                        youtubeRegions.Add(youtubeRegion);
                        break;
                    case RegionType.Blog:
                        var blogRegion = new BlogRegion(region);
                        blogRegion.LoadData(rootPath, region.mappedObject, site, parameter, friendlyUrl);
                        blogRegions.Add(blogRegion);
                        break;
                    case RegionType.Map:
                        var mapRegion = new MapRegion(region);
                        mapRegion.LoadData(site, rootPath, title, region.mappedObject);
                        mapRegions.Add(mapRegion);
                        break;
                }
            }
        }

        public void LoadPageDataFromDb(string rootPath, string site, string pageName, Template template)
        {
            this.template = template.template;
            int sequence = 0;
            foreach (var region in template.regions)
            {
                region.sequence = sequence;
                this.regions.Add(region);
                sequence++;
            }

            RepoBase repo = RepoBase.RepoFactory(site, context);

            if (repo == null)
            {
                throw new Exception("Repo not defined");
            }

            foreach (var region in regions)
            {
                switch (region.regionType)
                {
                    case RegionType.Gallery:
                        var galleryRegion = repo.GetGalleryFromDb(region, pageName, rootPath);
                        galleryRegions.Add(galleryRegion);
                        break;
                    case RegionType.Html:
                        var htmlRegion = repo.GetHtmlFromDb(region, pageName);
                        htmlRegions.Add(htmlRegion);
                        break;
                }
            }

        }

        public List<RegionBase> GetRegionAreas(string regionName)
        {
            var region = this.regions.Where(x => x.templateTag == regionName);
            return region.ToList();
        }

        public GalleryRegion GetGallery(string galleryName)
        {
            var gallery = this.galleryRegions.Where(x => x.mappedObject == galleryName).FirstOrDefault();
            return gallery;
        }

        public BlogRegion GetBlog(string blogName)
        {
            var blog = this.blogRegions.Where(x => x.mappedObject == blogName).FirstOrDefault();
            return blog;
        }

        public YouTubeRegion GetYoutube(string youtubeName)
        {
            var youtube = this.youtubeRegions.Where(x => x.mappedObject == youtubeName).FirstOrDefault();
            return youtube;
        }


        public HtmlRegion GetHtml(string htmlName)
        {
            var html = this.htmlRegions.Where(x => x.mappedObject == htmlName).FirstOrDefault();
            return html;
        }

        public MapRegion GetMap(string mapName)
        {
            var map = this.mapRegions.Where(x => x.mappedObject == mapName).FirstOrDefault();
            return map;
        }

    }
}
