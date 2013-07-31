﻿using Newtonsoft.Json;
using oAuthTwitterWrapper;

namespace OAuthTwitterWrapper
{
    public class OAuthTwitterWrapper
    {
        private const string OAuthConsumerKey = "dlwIzwzXwBTY0BiPei3Yg";
        private const string OAuthConsumerSecret = "dorC5hwB8t0a3HlYbMkuW0tN2QClgdI8pahfRMEcJ8";
        private const string OAuthUrl = "https://api.twitter.com/oauth2/token";
        private string timelineUrl; 
        private const string SearchFormat = "https://api.twitter.com/1.1/search/tweets.json?q={0}";
        private const string SearchQuery = "%23test";
		private readonly string searchUrl = string.Format(SearchFormat, SearchQuery);

        public dynamic GetMyTimeline(string url)
        {
            timelineUrl = url;
            var authenticate = new Authenticate();
			TwitAuthenticateResponse twitAuthResponse = authenticate.AuthenticateMe(OAuthConsumerKey, OAuthConsumerSecret, OAuthUrl);

            // Do the timeline
			var timeLineJson = new Utility().RequstJson(timelineUrl, twitAuthResponse.token_type, twitAuthResponse.access_token);
 
            if(twitAuthResponse.error!=null)
                return twitAuthResponse.error;
            
            if (timeLineJson == string.Empty || timeLineJson == "[]")
                return null;

            dynamic result = JsonConvert.DeserializeObject<dynamic>(timeLineJson);  
            return result;
        }

		public string GetSearch()
		{
		    var authenticate = new Authenticate();
			TwitAuthenticateResponse twitAuthResponse = authenticate.AuthenticateMe(OAuthConsumerKey, OAuthConsumerSecret, OAuthUrl);

			// Do the timeline
			var utility = new Utility();
			string searchJson = utility.RequstJson(searchUrl, twitAuthResponse.token_type, twitAuthResponse.access_token);

			return searchJson;
		}
    }
}
