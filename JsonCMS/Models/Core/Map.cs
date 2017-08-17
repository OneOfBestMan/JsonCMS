using JsonCMS.Models.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Core
{
    public class Map
    {
        public string mapId { get; set; }

        public string mapHtml = string.Empty; 

        public void LoadData(string site, string rootPath, string pageName, string regionName)
        {
            var json = new Json<String>(rootPath); // ok not json
            var html = json.ReadFile(site + "/CMSdata/pages/" + pageName, regionName + ".html");
            this.mapHtml = html;
        }
    }
}
