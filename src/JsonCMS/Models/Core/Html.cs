using JsonCMS.Models.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Core
{
    public class Html
    {
        public Html()
        {
        }

        public Html(string html)
        {
            this.html = html;
        }

        public string html = string.Empty; 

        public void LoadHtml(string site, string rootPath, string pageName, string regionName)
        {
            var json = new Json<String>(rootPath); // ok not json
            var html = json.ReadFile(site + "/CMSdata/pages/" + pageName, regionName + ".html");
            this.html = html;
        }
    }
}
