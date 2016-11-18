using JsonCMS.Models.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.PageModels
{

    public class Page
    {
        public string title { get; set; }

        public string displayName { get; set; }

        public string friendlyUrl { get; set; }

        private PageType _pageType = PageType.Standard;

        public string template
        {
            set
            {
                switch (value.ToLower())
                {
                    case "blog": _pageType = PageType.Blog; break;
                    case "twocolumn": _pageType = PageType.TwoColumn; break;
                    case "standard": _pageType = PageType.Standard; break;
                    case "photosection": _pageType = PageType.PhotoSection; break;
                    case "carousel": _pageType = PageType.Carousel; break;
                    case "miniatures": _pageType = PageType.Miniatures; break;
                }
            }
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

    }
}
