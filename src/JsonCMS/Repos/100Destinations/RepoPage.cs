using System;
using System.Collections.Generic;

namespace JsonCMS.Repos._Top100
{
    public partial class RepoPage
    {
        public int id { get; set; }
        public string location { get; set; }
        public string wikipediaPageTitle { get; set; }
        public string cleaneddatax { get; set; }
        public string continent { get; set; }
        public string type { get; set; }
        public Nullable<int> views { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string country { get; set; }
        public string cleaneddata { get; set; }
        public Nullable<int> googlemapzoom { get; set; }
        public string twitteralternatives { get; set; }
        public Nullable<System.DateTime> lastdateoftweets { get; set; }
    }
}
