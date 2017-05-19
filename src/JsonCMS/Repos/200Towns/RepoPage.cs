using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Repos._200Towns
{
    public class RepoPage
    {
        public int id { get; set; }
        public string town { get; set; }
        public bool city { get; set; }
        public Nullable<double> Population { get; set; }
        public string county { get; set; }
        public string wikipediaPageTitle { get; set; }
        public string townAndCounty { get; set; }
        public string country { get; set; }
        public string cleaneddata { get; set; }
        public string region { get; set; }
        public Nullable<int> views { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public Nullable<System.DateTime> lastdateoftweets { get; set; }
        public string grouping { get; set; }
    }
}
