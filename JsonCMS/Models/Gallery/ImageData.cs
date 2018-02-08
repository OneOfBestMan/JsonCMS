using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Galleries
{

    public class ImageData
    {
        public string imageName { get; set; }
        public string title { get; set; }
        public string alt { get; set; }
        public string url { get; set; }
        public string author { get; set; }
        public string description { get; set; }
        public bool fileExists { get; set; } = false;
        public bool lightBox { get; set; } = false;
        public int? height { get; set; } 
        public int? width { get; set; }
        public DateTime dateAdded { get; set; }

        [JsonIgnore]
        public bool serialiseVersions { get; set; } = false;

        public List<ImageVersion> Versions { get; set; } = new List<ImageVersion>();

        public bool ShouldSerializeVersions()
        {
            return serialiseVersions;
        }
    }

}
