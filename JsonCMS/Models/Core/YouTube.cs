using JsonCMS.Models.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Core
{
    public class YouTube
    {

        public List<Video> videos { get; set; }

        public void LoadVideos(string site, string rootPath, string pageName, string regionName)
        {
            var json = new Json<YouTube>(rootPath); // ok not json
            var videos = json.ReadJsonObject(json.ReadFile(site + "/CMSdata/pages/" + pageName, regionName + ".json"));
            this.videos = videos.videos;
        }
    }

    public class Video
    {
        public string video { get; set; }
        public string videoFormat { get; set; } = "16by9";
    }

}
