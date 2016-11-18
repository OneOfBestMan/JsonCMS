using JsonCMS.Models.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Galleries
{
    public class ImageSize
    {
        public int width { get; set; }
        public int height { get; set; }
        public CropType cropType { get; set; }
        public Aspect originalAspect { get; set; }

        public ImageSize(int height, int width, CropType cropType)
        {
            this.height = height;
            this.width = width;
            this.cropType = cropType;

            if (height > width)
            {
                originalAspect = Aspect.Portrait;
            }
            else
            {
                if (width > height)
                {
                    originalAspect = Aspect.Landscape;
                }
                else
                {
                    originalAspect = Aspect.Square;
                }
            }
        }

        public static ImageSize CalculateSize(ImageSize originalImageSize, int maxHeight, int maxWidth, int imagesAcross, CropType cropType)
        {
            switch (cropType)
            {
                case CropType.Square:

                    if (maxHeight == 0) { maxHeight = maxWidth; }
                    if (maxWidth == 0) { maxWidth = maxHeight; }

                    double size = maxHeight > maxWidth ? maxHeight : maxWidth;
                    size = (int)Math.Ceiling(size / imagesAcross);
                    double largerSide = originalImageSize.height > originalImageSize.width ? originalImageSize.height : originalImageSize.width;
                    size = largerSide < size ? largerSide : size;
                    return new ImageSize((int)size, (int)size, cropType);

                case CropType.Panoramic:

                    /* is width based */
                    if (maxWidth == 0) { maxWidth = (int)Math.Ceiling((double)maxHeight / Graphics.panoramicAspectRatio); }
                    maxHeight = (int)(maxWidth * Graphics.panoramicAspectRatio);

                    return new ImageSize((int)Math.Ceiling((double)maxHeight / imagesAcross), (int)Math.Ceiling((double)maxWidth / imagesAcross), cropType);

                default:
                    // need to retain aspect ratios

                    float aspectRatio = (float)originalImageSize.height / (float)originalImageSize.width;

                    if (maxHeight == 0) { maxHeight = (int)Math.Ceiling((double)maxWidth * aspectRatio); } // allow 0 if only want to constrain on one dimension
                    if (maxWidth == 0) { maxWidth = (int)Math.Ceiling((double)maxHeight / aspectRatio); }

                    if (originalImageSize.originalAspect == Aspect.Landscape)
                    {
                        maxWidth = (int)(maxHeight / aspectRatio);
                    }
                    else
                    {
                        maxHeight = (int)(maxWidth * aspectRatio);
                    }
                    return new ImageSize((int)Math.Ceiling((double)maxHeight / imagesAcross), (int)Math.Ceiling((double)maxWidth / imagesAcross), cropType);
            }
        }
    }

}
