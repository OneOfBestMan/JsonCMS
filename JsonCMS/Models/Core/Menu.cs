﻿using JsonCMS.Models.Libraries;
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
        public bool nestedMenu { get; set; } = false;
        public bool menuFromPages { get; set; } = false;
        public List<string> groups { get; set; } = new List<string>();
    }

    public class Menus
    {
        private const int maxMenus = 5;

        public List<Menu> menus { get; set; } = new List<Menu>();

        public void LoadMenus(string rootPath, Pages pages, string site, bool loadPagesFromDb)
        {
            for (var i = 1; i <= maxMenus; i++)
            {
                var menuJson = new Json<Menu>(rootPath);
                if (menuJson.FileExists(site + "/CMSdata/menus", "menu" + i + ".json"))
                {
                    var menu = menuJson.ReadJsonObject(menuJson.ReadFile(site + "/CMSdata/menus", "menu" + i + ".json"));
                    menu.menuName = "menu" + i;
                    menu.siteTag = site;

                    if (menu.menuFromPages)
                    {
                        loadPagesIntoMenu(menu, pages);
                        if (menu.nestedMenu)
                        {
                            loadGroupsIntoMenu(menu, pages);
                        }
                    }

                    foreach (var menuItem in menu.menu)
                    {
                        menuItem.page = FindPage(menuItem.pagetitle, pages);
                    }

                    this.menus.Add(menu);
                }
            }
        }

        private void loadGroupsIntoMenu(Menu menu, Pages pages)
        {
            Dictionary<string, int> groups = new Dictionary<string, int>();
            foreach (var page in pages.pages)
            {
                var group = page.grouping;
                if (group != null)
                {
                    if (!groups.Keys.Contains(group))
                    {
                        groups.Add(group, 0);
                    }
                }
            }
            foreach (var group in groups.OrderBy(x => x.Key))
            {
                menu.groups.Add(group.Key);
            }
        }

        private void loadPagesIntoMenu (Menu menu, Pages pages)
        {
            foreach (var page in pages.pages)
            {
                if (page.pageType != PageType.Home)
                {
                    MenuItem item = new MenuItem();
                    item.pagetitle = page.title;
                    menu.menu.Add(item);
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
