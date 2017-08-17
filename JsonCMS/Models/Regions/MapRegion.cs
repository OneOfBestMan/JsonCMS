using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonCMS.Models.Core;

namespace JsonCMS.Models.Regions
{
    public class MapRegion : RegionBase
    {
        public Map data = null;

        public MapRegion(RegionBase regionBase)
        {
            this.sequence = regionBase.sequence;
            this.templateTag = regionBase.templateTag;
            this.mappedObject = regionBase.mappedObject;
            this.SetType(regionBase.regionType);
            this.title = regionBase.title;
        }

        public void LoadData(string site, string rootPath, string pageName, string regionName)
        {
            data = new Map();
            data.LoadData(site, rootPath, pageName, regionName);
        }
    }
}
