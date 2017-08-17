using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JsonCMS.Models.Core;
using JsonCMS.Models.PageModels;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace JsonCMS.Controllers
{
    [Route("api/[controller]")]
    public class MapApiController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        private string rootPath;

        public MapApiController(IHostingEnvironment hostingEnvironment)
        {
            _appEnvironment = hostingEnvironment;
            rootPath = _appEnvironment.ContentRootPath + "/wwwroot";
        }

        [HttpGet]
        public MapDto GetMap([FromServices]dbContext context, string thislocation, string d = null)
        {
            MapDto mapDto = new MapDto();
            List<MarkerDto> markers = new List<MarkerDto>();
            var pages = new Pages(context);
            pages.LoadPages(rootPath, d, true);

            int c = 0;
            foreach (var page in pages.pages)
            {
                if (page.location != null && page.location.longitude != string.Empty)
                {
                    MarkerDto marker = new MarkerDto(page.location.latitude,
                        page.location.longitude, page.friendlyUrl, page.displayName);
                    markers.Add(marker);
                    if (thislocation == page.displayName)
                    {
                        mapDto.mapzoom = page.location.zoom;
                        mapDto.thisLocation = c;
                    }
                    c++;
                }
            }

            mapDto.markers = markers;
            return mapDto;
        }
    }

    public class MapDto
    {
        public List<MarkerDto> markers = new List<MarkerDto>();
        public int mapzoom;
        public int thisLocation;
    }

    public class MarkerDto
    {
        public MarkerDto(string latitute, string longitude, string url, string location)
        { 
            this.Latitude = latitute;
            this.Longitude = longitude;
            this.Location = location;
            this.Url = url;
        }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Location { get; set; }

        public string Url { get; set; }
    }
}
