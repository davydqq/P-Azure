using Microsoft.ProjectOxford.Text.Sentiment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloTextAnalytics
{
	static class SentimentAnalysis
	{
		public static async Task AnalyzeSentimentAsync(string id, string text, string language)
		{
			var document = new SentimentDocument()
			{
				Id = id,
				Text = text,
				Language = language
			};

			var client = new SentimentClient(Constants.API_KEY);
			var request = new SentimentRequest();

			request.Documents.Add(document);

			try
			{
				var response = await client.GetSentimentAsync(request);

				Console.WriteLine("------------------------");
				Console.WriteLine("   SENTIMENT ANALYSIS   ");
				Console.WriteLine("------------------------");

				foreach (var doc in response.Documents)
				{
					Console.WriteLine("   Sentiment Score: {0}%", (doc.Score * 100));
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
