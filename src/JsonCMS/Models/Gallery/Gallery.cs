using ImageMagick;
using JsonCMS.Models.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Galleries
{
    public class Gallery
    {
        protected CropType _cropType = CropType.None;
        protected CropFrom _cropFrom = CropFrom.Center;

        public int desktopImagesAcrossPage { get; set; }
        public int mobileImagesAcrossPage { get; set; }
        public int maxDefaultWidth { get; set; }
        public int maxPopupWidth { get; set; } = 700;
        public int maxDefaultHeight { get; set; }
        public int maxMobileWidth { get; set; }
        public int maxMobileHeight { get; set; }

        public float spaceBetween { get; set; } = 10; // percent

        protected string rootPath;
        protected string galleryPath;
        protected string site;

        protected bool mobileLightBoxRequired = false;

        public string cropType
        {
            set
            {
                switch (value.ToLower())
                {
                    case "square": _cropType = CropType.Square; break;
                    case "panoramic": _cropType = CropType.Panoramic; break;
                }
            }

            get
            {
                return _cropType.ToString().ToLower();
            }
        }

        public string cropFrom
        {
            set
            {
                switch (value.ToLower())
                {
                    case "top": _cropFrom = CropFrom.Top; break;
                    case "bottom": _cropFrom = CropFrom.Bottom; break;
                    case "left": _cropFrom = CropFrom.Left; break;
                    case "right": _cropFrom = CropFrom.Right; break;
                    default:
                        _cropFrom = CropFrom.Center; break;
                }
            }

            get
            {
                return _cropFrom.ToString().ToLower();
            }
        }

        [JsonIgnore]
        public CropType crop
        {
            get
            {
                return _cropType;
            }
        }

        [JsonIgnore]
        public CropFrom cropfrom
        {
            get
            {
                return _cropFrom;
            }
        }

        public List<ImageData> imageData { get; set; }

        public void LoadGallery(string rootPath, string galleryName, string site)
        {
            
            this.rootPath = rootPath;
            this.galleryPath = site + "/CMSdata/galleries/" + galleryName;
            this.site = site;

            var galleryJson = new Json<Gallery>(rootPath);
            var gallery = galleryJson.ReadJsonObject(galleryJson.ReadFile(galleryPath, "gallery.json"));
            this.imageData = gallery.imageData;
            this.desktopImagesAcrossPage = gallery.desktopImagesAcrossPage;
            this.mobileImagesAcrossPage = gallery.mobileImagesAcrossPage;
            this.maxDefaultWidth = gallery.maxDefaultWidth;
            this.maxPopupWidth = gallery.maxPopupWidth;
            this.maxDefaultHeight = gallery.maxDefaultHeight;
            this.maxMobileWidth = gallery.maxMobileWidth;
            this.maxMobileHeight = gallery.maxMobileHeight;
            this.cropType = gallery.crop.ToString();
            this.cropFrom = gallery.cropfrom.ToString();
            this.spaceBetween = gallery.spaceBetween;

            var filesInFolder = Files.GetFiles(rootPath + "/" + this.galleryPath, ".jpg");
            if (CheckForMissingImages(filesInFolder))
            {
                SaveUpdatedImagesJson();
            }

            LoadImageVersions();
        }

        protected void LoadImageVersions()
        {
            if (this.imageData != null)
            {
                foreach (var image in this.imageData.Where(x => x.fileExists))
                {
                    if (image.height == null || image.width == null)
                    {
                        GetOriginalSize(image);
                    }

                    ImagePath originalImagePaths = new ImagePath(rootPath, this.galleryPath, image.imageName);
                    ImageSize originalImageSize = new ImageSize(image.height ?? 0, image.width ?? 0, CropType.None);
                    ImageVersion originalImage = new ImageVersion(ImageVersionTypes.OriginalSize, originalImageSize, originalImagePaths);
                    image.Versions.Add(originalImage);

                    if (image.lightBox)
                    {
                        var defaultWidth = this.maxDefaultWidth < this.maxPopupWidth ? this.maxDefaultWidth : this.maxPopupWidth;
                        ImageSize desktopImageSize = ImageSize.CalculateSize(originalImageSize, 0, defaultWidth, 1, CropType.None);
                        ImagePath desktopImagePaths = new ImagePath(rootPath,
                            this.site + "/images", "thumb_h" + desktopImageSize.height + "_w" + desktopImageSize.width + "_" + image.imageName, originalImage);
                        ImageVersion desktopMaxImage = new ImageVersion(ImageVersionTypes.DesktopMaxSize, desktopImageSize, desktopImagePaths);
                        desktopMaxImage.CreateThumbnail();
                        image.Versions.Add(desktopMaxImage);

                        if (mobileLightBoxRequired)
                        {
                            ImageSize mobileMaxImageSize = ImageSize.CalculateSize(originalImageSize, 0, this.maxMobileWidth, 1, CropType.None);
                            ImagePath mobileImagePaths = new ImagePath(rootPath,
                                this.site + "/images", "thumb_h" + mobileMaxImageSize.height + "_w" + mobileMaxImageSize.width + "_" + image.imageName, originalImage);
                            ImageVersion mobileMaxImage = new ImageVersion(ImageVersionTypes.MobileMaxSize, mobileMaxImageSize, mobileImagePaths);
                            mobileMaxImage.CreateThumbnail();
                            image.Versions.Add(mobileMaxImage);
                        }
                    }

                    ImageSize desktopForGalleryImageSize = ImageSize.CalculateSize(originalImageSize, this.maxDefaultHeight,
                        this.maxDefaultHeight, this.desktopImagesAcrossPage, this.crop);
                    ImagePath desktopForGalleryImagePaths = new ImagePath(rootPath, this.site + "/images",
                        "thumb_h" + desktopForGalleryImageSize.height + "_w" + desktopForGalleryImageSize.width + "_" + image.imageName, originalImage);
                    ImageVersion desktopForGalleryImage = new ImageVersion(ImageVersionTypes.DesktopForGallery, desktopForGalleryImageSize, desktopForGalleryImagePaths);
                    desktopForGalleryImage.CreateThumbnail(this.cropfrom);
                    image.Versions.Add(desktopForGalleryImage);

                    ImageSize mobileForGalleryImageSize = ImageSize.CalculateSize(originalImageSize, this.maxDefaultHeight,
                        this.maxMobileHeight, this.mobileImagesAcrossPage, this.crop);
                    ImagePath mobileForGalleryImagePaths = new ImagePath(rootPath, this.site + "/images",
                        "thumb_h" + mobileForGalleryImageSize.height + "_w" + mobileForGalleryImageSize.width + "_" + image.imageName, originalImage);
                    ImageVersion mobileForGalleryImage = new ImageVersion(ImageVersionTypes.MobileForGallery, mobileForGalleryImageSize, mobileForGalleryImagePaths);
                    mobileForGalleryImage.CreateThumbnail(this.cropfrom);
                    image.Versions.Add(mobileForGalleryImage);
                }
            }
        }

        protected bool CheckForMissingImages(List<string> filenames)
        {
            var addedInfo = 0;
            foreach (string filename in filenames)
            {
                if (this.imageData.Where(x => x.imageName.ToLower() == filename.ToLower()).Count()==0)
                {
                    ImageData newItem = new ImageData();
                    newItem.imageName = filename;
                    newItem.fileExists = true;
                    this.imageData.Add(newItem);
                    addedInfo++;
                }

                var thisImage = this.imageData.Where(x => x.imageName.ToLower() == filename.ToLower()).FirstOrDefault();
                if (thisImage != null)
                {
                    thisImage.fileExists = true;

                    if (thisImage.height == null || thisImage.width == null)
                    {
                        GetOriginalSize(thisImage);
                        addedInfo++;
                    }
                }

            }

            return addedInfo > 0;
        }

        private void GetOriginalSize(ImageData image)
        {
            Graphics gr = new Graphics();
            MagickImageInfo im = gr.GetImageInfo(rootPath + "/" + this.galleryPath + "/" + image.imageName);
            image.height = im.Height;
            image.width = im.Width;
        }

        private void SaveUpdatedImagesJson ()
        {
            var galleryJson = new Json<Gallery>(rootPath);
            var json = galleryJson.FormattedJson(this);
            var gallery = galleryJson.WriteJson(json, galleryPath, "gallery.json", true);
        }

    }





}
