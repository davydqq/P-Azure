using Microsoft.ProjectOxford.Text.Core;
using Microsoft.ProjectOxford.Text.KeyPhrase;
using Microsoft.ProjectOxford.Text.Language;
using Microsoft.ProjectOxford.Text.Sentiment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitterMonitoring
{
	public static class SentimentService
	{
		public static float AnalyzeSentiment(string id, string text, string language)
		{
			float score = 0;
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
				var response = client.GetSentiment(request);
				score = response.Documents.First().Score * 100;
			}
			catch (Exception ex)
			{
				var message = "";
				var innerMessage = "";
				if (!String.IsNullOrEmpty(ex.Message))
					message = ex.Message;

				try
				{
					if ((ex.InnerException != null) && (!String.IsNullOrEmpty(ex.InnerException.Message)))
						innerMessage = ex.InnerException.Message;

				}
				catch (Exception innerEx)
				{
					if ((innerEx.InnerException != null) && (!String.IsNullOrEmpty(innerEx.InnerException.Message)))
						innerMessage = innerEx.InnerException.Message;
				}

				Console.WriteLine(String.Format("Error in AnalyzeSentiment: {0}:{1}", message, innerMessage));
			}
			return score;
		}

		public static IList<string> AnalyzeKeyPhrases(string id, string text, string language)
		{
			IList<string> keyPhrases = new List<string>();
			var document = new KeyPhraseDocument()
			{
				Id = id,
				Text = text,
				Language = language
			};

			var client = new KeyPhraseClient(Constants.API_KEY);

			var request = new KeyPhraseRequest();
			request.Documents.Add(document);

			try
			{
				var response = client.GetKeyPhrases(request);
				var doc = response.Documents.First();
				foreach (var keyPhrase in doc.KeyPhrases)
				{
					keyPhrases.Add(keyPhrase);
				}
			}
			catch (Exception ex)
			{
				var message = "";
				var innerMessage = "";
				if (!String.IsNullOrEmpty(ex.Message))
					message = ex.Message;

				try
				{
					if ((ex.InnerException != null) && (!String.IsNullOrEmpty(ex.InnerException.Message)))
						innerMessage = ex.InnerException.Message;

				}
				catch (Exception innerEx)
				{
					if ((innerEx.InnerException != null) && (!String.IsNullOrEmpty(innerEx.InnerException.Message)))
						innerMessage = innerEx.InnerException.Message;
				}

				Console.WriteLine(String.Format("Error in AnalyzeSentiment: {0}:{1}", message, innerMessage));
			}
			return keyPhrases;
		}

		public static DetectedLanguage AnalyzeLanguage(string id, string text)
		{
			DetectedLanguage language = new DetectedLanguage();
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
				var response = client.GetLanguages(request);
				language = response.Documents.First().DetectedLanguages.First();
			}
			catch (Exception ex)
			{
				var message = "";
				var innerMessage = "";
				if (!String.IsNullOrEmpty(ex.Message))
					message = ex.Message;

				try
				{
					if ((ex.InnerException != null) && (!String.IsNullOrEmpty(ex.InnerException.Message)))
						innerMessage = ex.InnerException.Message;

				}
				catch (Exception innerEx)
				{
					if ((innerEx.InnerException != null) && (!String.IsNullOrEmpty(innerEx.InnerException.Message)))
						innerMessage = innerEx.InnerException.Message;
				}

				Console.WriteLine(String.Format("Error in AnalyzeSentiment: {0}:{1}", message, innerMessage));
			}
			return language;
		}
	}
}

