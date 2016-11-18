﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using JsonCMS.Models.Libraries;
using JsonCMS.Models;
using JsonCMS.Models.Blogs;

namespace JsonCMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        private string baseSite = "json-cms.co.uk";

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _appEnvironment = hostingEnvironment;
        }

        public IActionResult Index(string id, string p) // p is optional parameter, called e.g. /blog?p=2
        {
            var pageName = id ?? "Index";
            var pageData = new JsonData();
            pageData.currentHost = HttpContext.Request.Host.Host; 
            pageData.LoadJsonForPage(pageName, _appEnvironment.ContentRootPath + "/wwwroot", p);

            if (ShowBaseSite(pageData))
            {
                return View();
            }

            ViewBag.LayoutData = new LayoutModel(pageData.currentSite, pageData.thisPage, pageData.currentDomain);
            string viewName = "~/Views/" + pageData.currentSite.viewFolder + "/" + pageData.thisPage.pageType.ToString() + ".cshtml";
            return View(viewName, pageData);
        }

        public IActionResult RebuildBlog(string id) // call using : /Home/RebuildBlog/blogX
        {
            var pageData = new JsonData();
            pageData.currentHost = HttpContext.Request.Host.Host;
            pageData.LoadJsonForPage("Index", _appEnvironment.ContentRootPath + "/wwwroot");

            BlogPage blogPage = new BlogPage();
            blogPage.RebuildBlog(_appEnvironment.ContentRootPath + "/wwwroot", id, pageData.currentSite.siteTag);

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private bool ShowBaseSite(JsonData pageData)
        {
            return (pageData.currentSite == null ||
                pageData.currentHost.IndexOf("www." + baseSite) == 0 || 
                pageData.currentHost.IndexOf(baseSite) == 0) ;
        }
    }
}