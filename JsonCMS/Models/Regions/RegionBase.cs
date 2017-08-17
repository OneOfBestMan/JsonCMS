using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Regions
{

    public class RegionBase
    {
        public int sequence = 0;
        private RegionType _regionType = RegionType.Unknown;

        public string templateTag { get; set; }
        public string mappedObject { get; set; }
        public string title { get; set; }

        public string type
        {
            set
            {
                switch (value.ToLower())
                {
                    case "blog": _regionType = RegionType.Blog; break;
                    case "gallery": _regionType = RegionType.Gallery; break;
                    case "html": _regionType = RegionType.Html; break;
                    case "latest": _regionType = RegionType.Latest; break;
                    case "youtube": _regionType = RegionType.YouTube; break;
                    case "map": _regionType = RegionType.Map; break;
                }
            }
        }

        public RegionType regionType
        {
            get
            {
                return _regionType;
            }
        }

        public void SetType(RegionType type)
        {
            _regionType = type;
        }
    }

}
