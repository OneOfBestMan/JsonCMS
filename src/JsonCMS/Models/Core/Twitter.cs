using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;

namespace JsonCMS.Models.Core
{
    public class Twitter
    {
        public Twitter(IConfiguration Configuration, string domain)
        {
            _consumerKey = Configuration.GetConnectionString(domain + "_twitterConsumerKey");
            _consumerSecret = Configuration.GetConnectionString(domain + "_twitterConsumerSecret");
            _accessToken = Configuration.GetConnectionString(domain + "_twitterAccessToken");
            _accessTokenSecret = Configuration.GetConnectionString(domain + "_twitterAccessTokenSecret");
        }

        private string _consumerKey;
        private string _consumerSecret;
        private string _accessToken;
        private string _accessTokenSecret;

        public IEnumerable<Tweetinvi.Models.ITweet> SearchTweets(string searchString)
        {
            Auth.SetUserCredentials(_consumerKey, _consumerSecret, _accessToken, _accessTokenSecret);
            var tweets = Search.SearchTweets(searchString).Where(t => useTweet(t.Text));

            return tweets;
        }

        public void sendtweet(string tweet)
        {
            Auth.SetUserCredentials(_consumerKey, _consumerSecret, _accessToken, _accessTokenSecret);
            Tweet.PublishTweet(tweet);
        }

        // shouldn't have to do this
        private bool useTweet(string tweet)
        {
            string upp1 = tweet.ToUpper();
            return ((tweet.IndexOf("is now listed on") < 1) &&
                        (upp1.IndexOf("PAEDO") < 0) && (upp1.IndexOf("PEADO") < 0) && (upp1.IndexOf("PAKI ") < 0) &&
                        (upp1.IndexOf("JOBS") < 0) && (upp1.IndexOf("4SQ.") < 0) &&
                        (upp1.IndexOf("FUCK") < 0) && (upp1.IndexOf("CUNT") < 0) &&
                        (upp1.IndexOf("#JOB") < 0) && (upp1.IndexOf("JOB_") < 0) &&
                         (upp1.IndexOf("RT ") < 0));
        }
    }
}
