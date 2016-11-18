using JsonCMS.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Regions
{
    public class HtmlRegion : RegionBase
    {
        public Html data = null;

        public HtmlRegion(RegionBase regionBase)
        {
            this.sequence = regionBase.sequence;
            this.templateTag = regionBase.templateTag;
            this.mappedObject = regionBase.mappedObject;
            this.SetType(regionBase.regionType);
            this.title = regionBase.title;
        }

        public void LoadData(string site, string rootPath, string pageName, string regionName)
        {
            data = new Html();
            data.LoadHtml(site, rootPath, pageName, regionName);
        }
    }
}
