using System;
using TwitterHelper.Models;

namespace TwitterMonitoring
{
	public class TwitterObserver : IObserver<Tweet>
	{
		public void OnCompleted()
		{
			Console.WriteLine("Done!");
		}

		public void OnError(Exception error)
		{
			Console.WriteLine(String.Format("Error: ", error.ToString()));
		}

		public void OnNext(Tweet tweet)
		{
			var value = new TwitterPayload(tweet);

			Console.WriteLine();
			Console.WriteLine("---------------------------------------------");
			Console.WriteLine(String.Format("   Tweet From {0} at {1}", value.ScreenName, value.CreatedAt.ToString()));
			Console.WriteLine("---------------------------------------------");
			Console.WriteLine(String.Format("   Name: {0}", value.Name));
			Console.WriteLine(String.Format("   Screen Name: {0}", value.ScreenName));
			Console.WriteLine(String.Format("   Text: {0}", value.Text));
			Console.WriteLine(String.Format("   Language: {0}", value.LanguageName));
			Console.WriteLine(String.Format("   Language Confidence: {0}%", value.LanguageConfidence));
			Console.WriteLine(String.Format("   Sentiment Score: {0}%", value.SentimentScore));
			Console.WriteLine("   Key Phrases:");
			foreach (var keyPhrase in value.KeyPhrases)
			{
				Console.WriteLine(String.Format("      {0}", keyPhrase));
			}
			Console.WriteLine(String.Format("   TimeZone: {0}", value.TimeZone));
			Console.WriteLine();
			Console.ReadLine();
		}
	}
}
