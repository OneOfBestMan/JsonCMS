using System;
using System.Collections.Generic;

namespace JsonCMS.Repos._GoByFerry
{
    public partial class RepoPage
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string WikipediaPageTitle { get; set; }
        public string Cleaneddatax { get; set; }
        public string Continent { get; set; }
        public string Type { get; set; }
        public int? Views { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Country { get; set; }
        public string Cleaneddata { get; set; }
        public int? Googlemapzoom { get; set; }
        public string Twitteralternatives { get; set; }
        public DateTime? Lastdateoftweets { get; set; }
    }
}
