using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloTextAnalytics
{
	class Program
	{
		static void Main(string[] args)
		{
			string id = Guid.NewGuid().ToString();
			Console.WriteLine("Enter text to analyze: "); // Prompt
			string text = Console.ReadLine();

			Console.WriteLine("------------------------");
			Console.WriteLine("   TEXT ANALYSIS   ");
			Console.WriteLine("------------------------");
			Console.WriteLine("   Document Id: {0}", id);
			Console.WriteLine("   Text Analyzed: {0}", text);

			SentimentAnalysis.AnalyzeSentimentAsync(id, text, "en").Wait();
			KeyPhraseAnalysis.AnalyzeKeyPhraseAsync(id, text, "en").Wait();

			Console.WriteLine();
			Console.WriteLine("Press any key to exit...");
			Console.ReadLine();
		}
	}
}
