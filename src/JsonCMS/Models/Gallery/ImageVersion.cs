using JsonCMS.Models.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Galleries
{
    public class ImageVersion
    {
        public ImageSize imageSize;
        public ImageVersionTypes versionType { get; set; }
        public ImagePath paths { get; set; }

        public ImageVersion(ImageVersionTypes versionType, ImageSize imageSize, ImagePath paths)
        {
            this.versionType = versionType;
            this.imageSize = imageSize;
            this.paths = paths;
        }

        public bool CreateThumbnail(CropFrom cropFrom = CropFrom.Center)
        {
            switch (this.imageSize.cropType)
            {
                case CropType.Square:
                    return Graphics.CreateSquareCroppedThumbnail(this.imageSize.height,
                       this.paths.originalImage.paths.diskFullPath, this.paths.diskFullPathBase64, cropFrom);
                case CropType.Panoramic:
                    return Graphics.CreatePanoramicThumbnail(this.imageSize.width, Graphics.panoramicAspectRatio, this.paths.originalImage.paths.diskFullPath, 
                        this.paths.diskFullPathBase64, cropFrom);
                default:
                    return Graphics.CreateThumbnailToWidthHeight(this.imageSize.width, this.imageSize.height,
                        this.paths.originalImage.paths.diskFullPath, this.paths.diskFullPathBase64);
            }
        }
    }



}
