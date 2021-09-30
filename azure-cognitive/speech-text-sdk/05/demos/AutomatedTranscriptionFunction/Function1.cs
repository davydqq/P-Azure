using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Logging;

namespace AutomatedTranscriptionFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run([BlobTrigger("audiofiles/{name}.wav",
            Connection = "AudioBlob")] Stream myBlob,
            [Blob("transcribedfiles/{name}.txt", FileAccess.Write,
            Connection = "TextBlob")] Stream textBlob,
            string name, ILogger log)
        {
            var config = SpeechConfig.FromSubscription(
                Environment.GetEnvironmentVariable("ApiKey",
                EnvironmentVariableTarget.Process),
                Environment.GetEnvironmentVariable("ApiRegion",
                EnvironmentVariableTarget.Process));

            var completionSource = new TaskCompletionSource<int>();

            using (var audioInput =
                AudioConfig.FromStreamInput(new AudioStreamReader(myBlob)))
            using (var recognizer = new SpeechRecognizer(config, audioInput))
            {
                var streamWriter = new StreamWriter(textBlob);

                recognizer.Recognized += (s, e) =>
                {
                    streamWriter.Write(e.Result);
                };

                recognizer.SessionStopped += (s, e) =>
                {
                    streamWriter.Flush();
                    streamWriter.Dispose();
                    completionSource.TrySetResult(0);
                };

                await recognizer.StartContinuousRecognitionAsync()
                    .ConfigureAwait(false);

                await Task.WhenAny(new[] { completionSource.Task });

                await recognizer.StopContinuousRecognitionAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}
