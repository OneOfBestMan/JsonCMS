using JsonCMS.Models.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Blogs
{
    public class BlogPage
    {
        public int entriesPerPage { get; set; } = 10;

        public string BlogName { get; set; }
        public int PageNo { get; set; } = 1;
        public int TotalEntries { get; set; } = 0;

        protected string rootPath;
        private string blogPath;
        protected string site;

        private List<BlogEntry> blogEntries = new List<BlogEntry>();

        public List<BlogEntry> pageEntries = new List<BlogEntry>();

        public void ReadPage(string rootPath, string blogName, string site, int pageNo)
        {
            this.BlogName = blogName;
            this.rootPath = rootPath;
            this.blogPath = site + "/CMSdata/blogs/" + blogName;
            this.site = site;
            this.PageNo = pageNo;

            var blogJson = new Json<BlogPage>(rootPath);
            if (blogJson.FileExists(this.blogPath, "page" + pageNo + ".json"))
            {
                var page = blogJson.ReadJsonObject(blogJson.ReadFile(this.blogPath, "page" + pageNo + ".json"));
                this.pageEntries = page.pageEntries;
                this.TotalEntries = page.TotalEntries;
            }
        }

        public void RebuildBlog(string rootPath, string blogName, string site)
        {
            this.BlogName = blogName;
            this.rootPath = rootPath;
            this.blogPath = site + "/CMSdata/blogs/" + blogName;
            this.site = site;

            var entryList = EntryList();

            foreach (var entry in entryList)
            {
                var _entry = new BlogEntry();
                _entry.LoadBlogEntry(rootPath, blogName, entry.Value, site);
                blogEntries.Add(_entry);
            }
            SaveUpdatedBlogJson();
        }

        private List<KeyValuePair<int, string>> EntryList()
        {
            return Files.GetSubFolders(rootPath + "/" + this.blogPath);
        }

        private void SaveUpdatedBlogJson()
        {
            RemoveOldPages();

            this.TotalEntries = blogEntries.Where(x=>x.exists).Count();
            var blogJson = new Json<BlogPage>(rootPath);
            int pages = (int)Math.Ceiling(((decimal)this.TotalEntries / entriesPerPage));
            for (var page = 1; page <= pages; page++)
            {
                pageEntries = blogEntries.OrderByDescending(x=>x.entryDate).Skip((page - 1) * entriesPerPage).Take(entriesPerPage).ToList();
                this.PageNo = page;
                foreach (var entry in this.pageEntries)
                {
                    entry.imageData.ForEach(x => x.serialiseVersions = true);
                }
                var json = blogJson.FormattedJson(this);
                var blog = blogJson.WriteJson(json, blogPath, "page" + page + ".json", true);
            }
        }

        private void RemoveOldPages()
        {
            var directoryPath = rootPath + "/" + blogPath + "/";
            var deleted = Files.DeleteFiles(directoryPath, "page*.json");
        }
    }
}
