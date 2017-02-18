using JsonCMS.Models.Libraries;
using JsonCMS.Models.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Core
{
    public class MenuItem
    {
        public string pagetitle { get; set; }
        public Page page { get; set; } = null;
    }

    public class Menu
    {
        public List<MenuItem> menu { get; set; }
        public string menuName { get; set; }
        public string menuType { get; set; }
        public string currentSelection { get; set; }
        public string siteTag { get; set; }
    }

    public class Menus
    {
        private const int maxMenus = 5;

        public List<Menu> menus { get; set; } = new List<Menu>();

        public void LoadMenus(string rootPath, Pages pages, string site)
        {
            for (var i = 1; i <= maxMenus; i++)
            {
                var menuJson = new Json<Menu>(rootPath);
                if (menuJson.FileExists(site + "/CMSdata/menus", "menu" + i + ".json"))
                {
                    var menu = menuJson.ReadJsonObject(menuJson.ReadFile(site + "/CMSdata/menus", "menu" + i + ".json"));
                    menu.menuName = "menu" + i;
                    menu.siteTag = site;
                    foreach (var menuItem in menu.menu)
                    {
                        menuItem.page = FindPage(menuItem.pagetitle, pages);
                        menuItem.page.friendlyUrl = menuItem.page.friendlyUrl + "?d=" + site;
                    }
                    this.menus.Add(menu);
                }
            }
        }

        public Menu GetMenu(string menuName, string currentPage)
        {
            var menu = this.menus.Where(x => x.menuName == menuName).FirstOrDefault();
            menu.currentSelection = currentPage;
            return menu;
        }

        private Page FindPage(string menuTitle, Pages pages) {
            var page = pages.pages.Where(x => x.title == menuTitle).FirstOrDefault();
            return page;
        }
    }
}
