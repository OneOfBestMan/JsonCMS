using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Galleries
{

    public enum CropType
    {
        Square,
        Panoramic,
        None
    }

    public enum Aspect
    {
        Square,
        Portrait,
        Landscape
    }

    public enum ImageVersionTypes
    {
        MobileMaxSize,
        MobileForGallery,
        DesktopMaxSize,
        DesktopForGallery,
        OriginalSize
    }

    public enum OrderGalleryBy
    {
        AsInFile,
        Height,
        Width, 
        Filename,
        Date,
        Caption,
        DateReverse
    }

}
