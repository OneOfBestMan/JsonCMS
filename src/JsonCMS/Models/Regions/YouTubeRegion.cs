using JsonCMS.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Regions
{
    public class YouTubeRegion : RegionBase
    {
        public YouTube data = null;

        public YouTubeRegion(RegionBase regionBase)
        {
            this.sequence = regionBase.sequence;
            this.templateTag = regionBase.templateTag;
            this.mappedObject = regionBase.mappedObject;
            this.SetType(regionBase.regionType);
            this.title = regionBase.title;
        }

        public void LoadData(string site, string rootPath, string pageName, string regionName)
        {
            data = new YouTube();
            data.LoadVideos(site, rootPath, pageName, regionName);
        }
    }

}
