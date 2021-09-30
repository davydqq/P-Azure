using Microsoft.CognitiveServices.Speech;
using System;
using System.Threading.Tasks;

namespace PluralsightSpeechToTextDemo
{
    class Program
    {
        public static async Task RecognizeSpeechAsync()
        {
            var config = SpeechConfig.FromSubscription("{ Your Key }", "{ Your Region }");

            using (var recognizer = new SpeechRecognizer(config))
            {
                Console.WriteLine("Say something...");

                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"Text recognized: {result.Text}");
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"No speech recognized");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellationDetails = CancellationDetails.FromResult(result);
                    Console.WriteLine($"Speech recognition canceled: {cancellationDetails.Reason}");

                    if (cancellationDetails.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"ErrorCode {cancellationDetails.ErrorCode}");
                        Console.WriteLine($"ErrorDetails {cancellationDetails.ErrorDetails}");
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            RecognizeSpeechAsync().Wait();
            Console.WriteLine("Please press a key to continue");
            Console.ReadLine();
        }
    }
}
