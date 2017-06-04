using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JsonCMS.Models.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace JsonCMS.Controllers
{
    [Route("api/[controller]")]
    public class TwitterApiController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IConfiguration _configuration;

        public TwitterApiController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public JsonResult GetTweets(string searchString, string d = null)
        {
            // DOCUMENTATION : http://james.newtonking.com/projects/json/help/index.html?topic=html/N_Newtonsoft_Json_Serialization.htm#

            if (string.IsNullOrEmpty(searchString))
            {
                return null;
            }

            Twitter twitter = new Twitter(_configuration, d);
            var tweets = twitter.SearchTweets(searchString);
            if (tweets == null) { return null; }
            return Json(tweets);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

    }
}
