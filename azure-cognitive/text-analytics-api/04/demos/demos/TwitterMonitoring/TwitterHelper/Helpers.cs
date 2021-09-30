using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TwitterHelper
{
	public static class Helpers
	{
		public static string getOAuthSignature(TwitterConfig config, string keywords, string oauth_nonce, string oauth_timestamp)
		{
			// Create Base oAuth String
			var baseString = string.Format(
				"oauth_consumer_key={0}" +
				"&oauth_nonce={1}" + 
				"&oauth_signature_method={2}" +
				"&oauth_timestamp={3}" +
				"&oauth_token={4}" +
				"&oauth_version={5}" + 
				"&track={6}",
				config.twitterConsumerKey,
				oauth_nonce,
				config.oAuthSignatureMethod,
				oauth_timestamp,
				config.twitterAccessToken,
				config.oAuthVersion,
				Uri.EscapeDataString(keywords));
			baseString = string.Concat("POST&", Uri.EscapeDataString(config.resourceUrl), "&", Uri.EscapeDataString(baseString));

			// Create Composite Key
			var compositeKey = string.Concat(Uri.EscapeDataString(config.twitterConsumerSecret), "&", Uri.EscapeDataString(config.twitterTokenSecret));

			// Create oAuth Signature
			string oauth_signature = string.Empty;
			using (var hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
			{
				oauth_signature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
			}

			return oauth_signature;
		}

		public static HttpWebRequest getHttpWebRequest(string resourceUrl, string authHeader)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resourceUrl);
			request.Headers.Add("Authorization", authHeader);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.PreAuthenticate = true;
			request.AllowWriteStreamBuffering = true;
			request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
			return request;
		}
	}
}
