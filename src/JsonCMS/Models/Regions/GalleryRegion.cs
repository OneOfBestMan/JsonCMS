using JsonCMS.Models.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Regions
{
    public class GalleryRegion : RegionBase
    {
        public Gallery data = null;

        public GalleryRegion(RegionBase regionBase)
        {
            this.sequence = regionBase.sequence;
            this.templateTag = regionBase.templateTag;
            this.mappedObject = regionBase.mappedObject;
            this.title = regionBase.title;
            this.SetType(regionBase.regionType);
        }

        public void LoadData(string rootPath, string galleryName, string site)
        {
            data = new Gallery();
            data.LoadGallery(rootPath, galleryName, site);
        }
    }

}
