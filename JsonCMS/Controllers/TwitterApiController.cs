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
        public void Post([FromBody]CommentDto comment)
        {
            if ("" + comment.surname + comment.address == "" && comment.name!="" && comment.comment !="")
            {
                Twitter twitter = new Twitter(_configuration, comment.d);

                string tweet = (comment.name + " says " + comment.comment + " #" + comment.location + " " + comment.url);
                if (tweet.Length > 140)
                {
                    tweet = tweet.Substring(0, 140);
                }
                twitter.sendtweet(tweet);

                // settings should be in config
                // await Mail.SendMail("bracketsfox@yahoo.com", "admin", "bracketsfox@googlemail.com", "Comment from " + comment.url, tweet);
            }
            else
            {
                throw new Exception("Failed");
            }
        }

    }

    public class CommentDto
    {
        public string comment { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string surname { get; set; }
        public string location { get; set; } 
        public string url { get; set; }
        public string d { get; set; }
    }
}
