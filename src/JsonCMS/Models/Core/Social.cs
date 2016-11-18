using JsonCMS.Models.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Core
{
    public class SocialNetworks
    {
        public List<SocialNetwork> social { get; set; }

        public void LoadSocialNetworks(string rootPath, string site)
        {
            var socialJson = new Json<SocialNetworks>(rootPath);
            var soc = socialJson.ReadJsonObject(socialJson.ReadFile(site + "/CMSdata", "social.json"));
            this.social = soc.social;
        }
    }

    public class SocialNetwork
    {
        public string altText { get; set; }
        public string icon { get; set; }
        public string url { get; set; }
        public string target { get; set; }
    }

}
