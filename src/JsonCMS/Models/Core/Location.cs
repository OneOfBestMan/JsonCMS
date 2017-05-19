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

        public Location(float latitude, float longitude, int zoom)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.zoom = zoom;
        }

        public float latitude { get; set; }
        public float longitude { get; set; }
        public int zoom { get; set; }
    }
}
