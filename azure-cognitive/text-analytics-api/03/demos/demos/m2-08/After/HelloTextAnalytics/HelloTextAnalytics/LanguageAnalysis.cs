using Microsoft.ProjectOxford.Text.Core;
using Microsoft.ProjectOxford.Text.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloTextAnalytics
{
	static class LanguageAnalysis
	{
		public static async Task AnalyzeLanguageAsync(string id, string text)
		{
			var document = new Document()
			{
				Id = id,
				Text = text
			};

			var client = new LanguageClient(Constants.API_KEY);

			var request = new LanguageRequest();
			request.Documents.Add(document);

			try
			{
				var response = await client.GetLanguagesAsync(request);

				Console.WriteLine("------------------------");
				Console.WriteLine("   LANGUAGE ANALYSIS    ");
				Console.WriteLine("------------------------");

				foreach (var doc in response.Documents)
				{
					foreach (var lang in doc.DetectedLanguages)
					{
						Console.WriteLine("   Language: {0}({1})", lang.Name, lang.Iso639Name);
						Console.WriteLine("   Confidence: {0}%", (lang.Score * 100));
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
