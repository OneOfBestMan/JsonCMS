using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonCMS.Models.Galleries;
using JsonCMS.Models.Libraries;

namespace JsonCMS.Models.Blogs
{
    public class BlogEntry : Gallery
    {
        // blog entry has set of images in gallery followed by an html region.

        private string entryPath;

        public string title { get; set; }
        public string html { get; set; } // have as separate file?
        public DateTime entryDate { get; set; } = DateTime.Now;

        public bool exists { get; set; } = false;

        public bool LoadBlogEntry(string rootPath, string blogName, string entryName, string site)
        {
            bool modified = false;
            exists = true;

            this.rootPath = rootPath;
            this.title = entryName;
            this.entryPath = site + "/CMSdata/blogs/" + blogName + "/" + entryName;
            this.galleryPath = this.entryPath; // gallery is in same folder
            this.site = site;
            this.imageData = new List<ImageData>();

            var blogJson = new Json<BlogEntry>(rootPath);
            var blog = blogJson.ReadJsonObject(blogJson.ReadFile(entryPath, "blog.json"));

            if (blog != null)
            {
                this.imageData = blog.imageData;
                this.desktopImagesAcrossPage = blog.desktopImagesAcrossPage;
                this.mobileImagesAcrossPage = blog.mobileImagesAcrossPage;
                this.maxDefaultWidth = blog.maxDefaultWidth;
                this.maxDefaultHeight = blog.maxDefaultHeight;
                this.maxMobileWidth = blog.maxMobileWidth;
                this.maxMobileHeight = blog.maxMobileHeight;
                this.cropType = blog.crop.ToString();
                this.cropFrom = blog.cropfrom.ToString();
                this.spaceBetween = blog.spaceBetween;
                this.entryDate = blog.entryDate;

                this.html = blog.html;
                if (this.entryDate.ToString("MMMM dd yyyy") == DateTime.Now.ToString("MMMM dd yyyy"))
                {
                    this.entryDate = blogJson.FileDate(entryPath, "blog.json");
                }
            }
            else
            {
                modified = true;
            }

            var filesInFolder = Files.GetFiles(rootPath + "/" + this.entryPath, ".jpg");
            if (CheckForMissingImages(filesInFolder))
            {
                modified = true;
            }

            LoadImageVersions();

            if (modified)
            {
                SaveUpdatedJson();
            }
            return modified;
        }

        private void SaveUpdatedJson()
        {
            var blogJson = new Json<BlogEntry>(rootPath);
            var json = blogJson.FormattedJson(this);
            var blog = blogJson.WriteJson(json, entryPath, "blog.json", true);
        }
    }
}
