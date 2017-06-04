using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Core
{
    public class Location
    {
        public Location()
        {
        }

        public Location(string latitude, string longitude, int zoom)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.zoom = zoom;
        }

        public string latitude { get; set; }
        public string longitude { get; set; }
        public int zoom { get; set; }
    }
}
