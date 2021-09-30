using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterHelper
{
	public class TwitterConfig
	{
		public string oAuthVersion { get; set; }
		public string oAuthSignatureMethod { get; set; }
		public string resourceUrl { get; set; }
		public string twitterConsumerKey { get; set; }
		public string twitterConsumerSecret { get; set; }
		public string twitterAccessToken { get; set; }
		public string twitterTokenSecret { get; set; }

		public TwitterConfig(string _twitterConsumerKey, string _twitterConsumerSecret, string _twitterAccessToken, string _twitterTokenSecret)
		{
			this.oAuthVersion = "1.0";
			this.oAuthSignatureMethod = "HMAC-SHA1";
			this.resourceUrl = "https://stream.twitter.com/1.1/statuses/filter.json";

			this.twitterConsumerKey = _twitterConsumerKey;
			this.twitterConsumerSecret = _twitterConsumerSecret;
			this.twitterAccessToken = _twitterAccessToken;
			this.twitterTokenSecret = _twitterTokenSecret;
		}
	}
}
