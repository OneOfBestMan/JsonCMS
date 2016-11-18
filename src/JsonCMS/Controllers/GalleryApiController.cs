using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JsonCMS.Models;
using Microsoft.AspNetCore.Hosting;
using JsonCMS.Models.Galleries;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace JsonCMS.Controllers
{
    [Route("api/[controller]")]
    public class GalleryApiController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;

        public GalleryApiController(IHostingEnvironment hostingEnvironment)
        {
            _appEnvironment = hostingEnvironment;
        }

        [HttpGet("{pageName}")]
        public IEnumerable<KeyValuePair<int, string>> GetPageGalleries(string pageName)
        {
            var pageData = new JsonData();
            pageData.currentHost = HttpContext.Request.Host.Host;
            pageData.LoadJsonForPage(pageName, _appEnvironment.ContentRootPath + "/wwwroot");

            List<KeyValuePair<int, string>> galleries = new List<KeyValuePair<int, string>>();
            int c = 0;
            foreach (var gallery in pageData.thisPage.galleryRegions)
            {
                c++;
                KeyValuePair<int, string> kp = new KeyValuePair<int, string>(c, gallery.title);
                galleries.Add(kp);
            }
            return galleries.ToArray();
        }

        [HttpGet("{pageName}/{galleryName}")]
        public IEnumerable<GalleryDTO> GetGallerySections(string pageName, string galleryName)
        {
            var pageData = new JsonData();
            pageData.currentHost = HttpContext.Request.Host.Host;
            pageData.LoadJsonForPage(pageName, _appEnvironment.ContentRootPath + "/wwwroot");

            var gallery = pageData.thisPage.galleryRegions.Where(x => x.title == galleryName).FirstOrDefault();
            List<GalleryDTO> fileNames = new List<GalleryDTO>();

            if (gallery != null)
            {
                foreach (var image in gallery.data.imageData)
                {
                    var rotation = 0;
                    var desktopGalleryImageVersion = image.Versions.Where(x => x.versionType == ImageVersionTypes.DesktopForGallery).FirstOrDefault();
                    if (desktopGalleryImageVersion != null) {
                        fileNames.Add(new GalleryDTO(rotation, desktopGalleryImageVersion.paths.wwwFullPathBase64, image.alt));
                    }
                }
            }

            return fileNames.OrderBy(a => Guid.NewGuid()).ToArray();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

    }

    public class GalleryDTO
    {
        public int key;
        public string value; // TO DO : change this
        public string alt;

        public GalleryDTO(int key, string value, string alt)
        {
            this.key = key;
            this.value = value;
            this.alt = alt;
        }
    }
}
