using System;
using System.Net;
using System.Text;
using System.IO;
using System.Web;
using System.Collections.Generic;
using TwitterHelper.Models;
using System.Runtime.Serialization.Json;

namespace TwitterHelper
{
	public static class Services
	{
		public static TextReader ReadTweets(TwitterConfig config, string keywords)
		{
			// unique request details
			var oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
			var oauth_timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).ToString();
			var oauth_signature = Helpers.getOAuthSignature(config, keywords, oauth_nonce, oauth_timestamp);

			// create the request header
			var authHeader = string.Format(
				"OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
				"oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
				"oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
				"oauth_version=\"{6}\"",
				Uri.EscapeDataString(oauth_nonce),
				Uri.EscapeDataString(config.oAuthSignatureMethod),
				Uri.EscapeDataString(oauth_timestamp),
				Uri.EscapeDataString(config.twitterConsumerKey),
				Uri.EscapeDataString(config.twitterAccessToken),
				Uri.EscapeDataString(oauth_signature),
				Uri.EscapeDataString(config.oAuthVersion)
			);

			// make the request
			ServicePointManager.Expect100Continue = false;
			HttpWebRequest request = Helpers.getHttpWebRequest(config.resourceUrl + "?track=" + HttpUtility.UrlEncode(keywords), authHeader);

			// bail out and retry after 5 seconds
			var tresponse = request.GetResponseAsync();
			if (tresponse.Wait(5000))
				return new StreamReader(tresponse.Result.GetResponseStream());
			else
			{
				request.Abort();
				return StreamReader.Null;
			}
		}

		public static IEnumerable<Tweet> StreamStatuses(TwitterConfig config, string keywords)
		{
			DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Tweet));

			var streamReader = ReadTweets(config, keywords);

			while (true)
			{
				string line = null;
				try { line = streamReader.ReadLine(); }
				catch (Exception) { }

				if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("{\"delete\""))
				{
					var result = (Tweet)jsonSerializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(line)));
					result.RawJson = line;
					yield return result;
				}

				// Oops the Twitter has ended... or more likely some error have occurred.
				// Reconnect to the twitter feed.
				if (line == null)
				{
					streamReader = ReadTweets(config, keywords);
				}
			}
		}

	}
}
