

using System.Runtime.Serialization;

namespace TwitterHelper.Models
{
	/// <summary>
	/// Class Model for Twitter Users Object (https://dev.twitter.com/overview/api/users)
	/// </summary>
	[DataContract]
	public class TwitterUser
	{
		/// <summary>
		/// Nullable . The user-defined location for this account’s profile. 
		/// Not necessarily a location, nor machine-parseable. This field will occasionally be fuzzily interpreted by the Search service.
		/// </summary>
		[DataMember(Name = "location")]
		public string Location;

		/// <summary>
		/// Nullable . A string describing the Time Zone this user declares themselves within. 
		/// </summary>
		[DataMember(Name = "time_zone")]
		public string TimeZone;

		/// <summary>
		/// When true, indicates that the user has a verified account.
		/// </summary>
		[DataMember(Name = "verified")]
		public bool Verified;

		/// <summary>
		/// The name of the user, as they’ve defined it. Not necessarily a person’s name. Typically capped at 20 characters, but subject to change. 
		/// </summary>
		[DataMember(Name = "name")]
		public string Name;

		/// <summary>
		/// The screen name, handle, or alias that this user identifies themselves with. screen_names are unique but subject to change. 
		/// Use id_str as a user identifier whenever possible. Typically a maximum of 15 characters long, but some historical accounts may exist with longer names. 
		/// </summary>
		[DataMember(Name = "screen_name")]
		public string ScreenName;
	}
}
