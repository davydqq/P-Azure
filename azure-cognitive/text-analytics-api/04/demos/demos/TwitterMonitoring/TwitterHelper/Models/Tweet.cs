using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TwitterHelper.Models
{
	/// <summary>
	/// Class Model for Twitter Tweets Object (https://dev.twitter.com/overview/api/tweets)
	/// </summary>
	[DataContract]
	public class Tweet
	{
		/// <summary>
		/// UTC time when this Tweet was created. 
		/// </summary>
		[DataMember(Name = "created_at")]
		public string CreatedAt;

		/// <summary>
		/// Nullable Indicates approximately how many times this Tweet has been liked by Twitter users.
		/// </summary>
		[DataMember(Name = "favorite_count")]
		public Int64? FavoriteCount;

		/// <summary>
		/// The integer representation of the unique identifier for this Tweet. 
		/// This number is greater than 53 bits and some programming languages may have difficulty/silent defects in interpreting it. 
		/// Using a signed 64 bit integer for storing this identifier is safe. Use id_str for fetching the identifier to stay on the safe side.
		/// </summary>
		[DataMember(Name = "id")]
		public Int64 Id;

		/// <summary>
		/// Nullable If the represented Tweet is a reply, this field will contain the screen name of the original Tweet’s author. 
		/// </summary>
		[DataMember(Name = "in_reply_to_screen_name")]
		public string ReplyToScreenName;

		/// <summary>
		/// Nullable If the represented Tweet is a reply, this field will contain the integer representation of the original Tweet’s ID.
		/// </summary>
		[DataMember(Name = "in_reply_to_status_id")]
		public Int64? ReplyToStatusId;

		/// <summary>
		/// Nullable If the represented Tweet is a reply, 
		/// this field will contain the integer representation of the original Tweet’s author ID. This will not necessarily always be the user directly mentioned in the Tweet.
		/// </summary>
		[DataMember(Name = "in_reply_to_user_id")]
		public Int64? ReplyToUserId;

		/// <summary>
		/// Nullable When present, indicates a BCP 47 language identifier corresponding to the machine-detected language of the Tweet text, or und if no language could be detected.
		/// </summary>
		[DataMember(Name = "lang")]
		public string Language;

		/// <summary>
		/// Number of times this Tweet has been retweeted.
		/// </summary>
		[DataMember(Name = "retweet_count")]
		public string RetweetCount;

		/// <summary>
		/// Utility used to post the Tweet, as an HTML-formatted string. Tweets from the Twitter website have a source value of web.
		/// </summary>
		[DataMember(Name = "source")]
		public string Source;

		/// <summary>
		/// The actual UTF-8 text of the status update. 
		/// </summary>
		[DataMember(Name = "text")]
		public string Text;

		/// <summary>
		/// The user who posted this Tweet. Perspectival attributes embedded within this object are unreliable.
		/// </summary>
		[DataMember(Name = "user")]
		public TwitterUser User;

		[IgnoreDataMember]
		public string RawJson;

	}
}
