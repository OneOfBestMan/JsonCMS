using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageMagick;

namespace JsonCMS.Models.Libraries
{
    public class Graphics
    {
        public static double panoramicAspectRatio = (double)0.675; /* height / width */

        public static bool CreatePanoramicThumbnail(int width, double aspectRatio, string serverpath, string thumbpath, CropFrom cropFrom = CropFrom.Center)
        {
            try
            {
                if (!File.Exists(thumbpath))
                {

                    using (MagickImage image = new MagickImage(serverpath))
                    {
                        /* first get crop shape */
                        int imageWidthToCropTo = image.Width;
                        int imageHeightToCropTo;
                        int requiredHeight = (int)(image.Width * aspectRatio);
                        if (image.Height < requiredHeight)
                        {
                            imageWidthToCropTo = (int)(image.Height / aspectRatio);
                            imageHeightToCropTo = image.Height;
                        }
                        else
                        {
                            imageHeightToCropTo = requiredHeight;
                        }

                        /* then get crop offsets */
                        int x = 0;
                        int y = 0;

                        switch (cropFrom)
                        {
                            case CropFrom.Center:
                                x = (image.Width - imageWidthToCropTo) / 2;
                                y = (image.Height - imageHeightToCropTo) / 2;
                                break;
                            case CropFrom.Bottom:
                                x = (image.Width - imageWidthToCropTo) / 2;
                                y = (image.Height - imageHeightToCropTo);
                                break;
                            case CropFrom.Top:
                                x = (image.Width - imageWidthToCropTo) / 2;
                                y = 0;
                                break;
                            case CropFrom.Left:
                                x = 0;
                                y = (image.Height - imageHeightToCropTo) / 2;
                                break;
                            case CropFrom.Right:
                                x = (image.Width - imageWidthToCropTo);
                                y = (image.Height - imageHeightToCropTo) / 2;
                                break;
                        }

                        MagickGeometry size = new MagickGeometry(x, y, imageWidthToCropTo, imageHeightToCropTo);
                        size.IgnoreAspectRatio = false;
                        image.Crop(size);

                        /* get final image at smaller size */
                        MagickGeometry finalsize = new MagickGeometry(0, 0, width, (int)(width * aspectRatio));
                        image.Resize(finalsize);

                        image.Write(thumbpath);
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool CreateSquareCroppedThumbnail(int dimension, string serverpath, string thumbpath, CropFrom cropFrom = CropFrom.Center)
        {
            try
            {
                if (!File.Exists(thumbpath))
                {

                    using (MagickImage image = new MagickImage(serverpath))
                    {
                        // need to resize to roughly right size then crop
                        int roughSize;
                        int x = 0;
                        int y = 0;

                        if (image.Height < image.Width)
                        {
                            roughSize = (int)((double)dimension / image.Height * image.Width);
                        }
                        else
                        {
                            roughSize = (int)((double)dimension / image.Width * image.Height);
                        }

                        MagickGeometry roughsize = new MagickGeometry(roughSize, roughSize);
                        image.Resize(roughsize);

                        switch (cropFrom) {
                            case CropFrom.Center:
                                if (image.Height < image.Width) // center crop
                                {
                                    x = (image.Width - image.Height) / 2;
                                }
                                else
                                {
                                    y = (image.Height - image.Width) / 2;
                                }
                                break;
                            case CropFrom.Bottom:
                                if (image.Height > image.Width) 
                                {
                                    y = (image.Height - image.Width);
                                }
                                break;
                            case CropFrom.Top:
                                // y = 0
                                break;
                            case CropFrom.Left:
                                // x = 0
                                break;
                            case CropFrom.Right:
                                if (image.Height < image.Width) 
                                {
                                    x = (image.Width - image.Height);
                                }
                                break;
                        }

                        MagickGeometry size = new MagickGeometry(x, y, dimension, dimension);
                        size.IgnoreAspectRatio = false;
                        image.Crop(size);

                        image.Write(thumbpath);
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public MagickImage GetImage(string path)
        {
            if (File.Exists(path))
            {
                MagickImage image = new MagickImage(path);
                return image;
            }
            else
            {
                return null;
            }
        }

        public MagickImageInfo GetImageInfo(string path)
        {
            if (File.Exists(path))
            {
                MagickImageInfo image = new MagickImageInfo(path);
                return image;
            }
            else
            {
                return null;
            }
        }

        public static bool CreateThumbnailToWidthHeight(int thumbnailwidth, int thumbnailheight, string serverpath, string thumbpath)
        {
            try
            {
                if (!File.Exists(thumbpath))
                {

                    using (MagickImage image = new MagickImage(serverpath))
                    {
                        double aspectRatio = (double)image.Width / (double)image.Height;
                        if (thumbnailheight == 0)
                        {
                            thumbnailheight = (int)(thumbnailwidth / aspectRatio);
                        }
                        if (thumbnailwidth == 0)
                        {
                            thumbnailwidth = (int)(thumbnailheight * aspectRatio);
                        }


                        MagickGeometry size = new MagickGeometry(thumbnailwidth, thumbnailheight);
                        size.IgnoreAspectRatio = false;
                        image.Resize(size);

                        image.Write(thumbpath);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }

    public enum CropFrom
    {
        Center,
        Left,
        Top,
        Right,
        Bottom
    }
}
