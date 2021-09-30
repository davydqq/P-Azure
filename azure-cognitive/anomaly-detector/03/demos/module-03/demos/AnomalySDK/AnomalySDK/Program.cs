using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.AnomalyDetector;
using Microsoft.Azure.CognitiveServices.AnomalyDetector.Models;

namespace AnomalySDK
{
    /// <summary>
    /// See 
    /// https://docs.microsoft.com/en-us/azure/cognitive-services/anomaly-detector/quickstarts/detect-data-anomalies-csharp-sdk 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string endpoint = "https://cs-ad-ps-demo01.cognitiveservices.azure.com/";
            string key = "92b04fc2e426432bbbe65ff4b5c4474b";

            // https://github.com/Azure-Samples/AnomalyDetector/blob/master/example-data/request-data.csv
            string timeSeriesDataPath = "time-series-data.csv";

            //Anomaly Detector client
            IAnomalyDetectorClient client = createClient(endpoint, key);

            // The request payload with points from the time series data file
            Request request = GetSeriesFromFile(timeSeriesDataPath);

            // Async method for BATCH anomaly detection
            EntireDetectSampleAsync(client, request).Wait(); 

            Console.WriteLine("-----------------------------------------------------------------------");

            // Async method for analyzing the LATEST data point in the set
            LastDetectSampleAsync(client, request).Wait();

            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Takes the API endpoint and key and creates an Anomaly Detector client
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        static IAnomalyDetectorClient createClient(string endpoint, string key)
        {
            IAnomalyDetectorClient client = 
                new AnomalyDetectorClient(new ApiKeyServiceClientCredentials(key))
            {
                Endpoint = endpoint
            };
            return client;
        }

        /// <summary>
        /// Takes a path to the time series data and prepares the data 
        /// for the Anomaly Detector API calls
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static Request GetSeriesFromFile(string path)
        {
            List<Point> list = File.ReadAllLines(path, Encoding.UTF8)
                .Where(e => e.Trim().Length != 0)
                .Select(e => e.Split(','))
                .Where(e => e.Length == 2)
                .Select(e => new Point(DateTime.Parse(e[0]), Double.Parse(e[1]))).ToList();

            return new Request(list, Granularity.Daily, sensitivity: 85);
        }

        /// <summary>
        /// Runs the Batch detection mode on the passed time series data
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        static async Task EntireDetectSampleAsync(IAnomalyDetectorClient client
            , Request request)
        {
            Console.WriteLine();
            Console.WriteLine("Detecting anomalies in the ENTIRE time series. (Batch detection mode)");

            EntireDetectResponse result = 
                await client.EntireDetectAsync(request).ConfigureAwait(false);

            if (result.IsAnomaly.Contains(true))
            {
                Console.WriteLine("An anomaly was detected at index:");
                for (int i = 0; i < request.Series.Count; ++i)
                {
                    if (result.IsAnomaly[i])
                    {
                        Console.Write(i);
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(" No anomalies detected in the series.");
            }
        }

        /// <summary>
        /// Runs the Stream detection mode on the passed time series data 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        static async Task LastDetectSampleAsync(IAnomalyDetectorClient client, Request request)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Detecting the anomaly status of the LATEST point in the series. (Stream detection mode)");

            LastDetectResponse result = 
                await client.LastDetectAsync(request).ConfigureAwait(false);

            if (result.IsAnomaly)
            {
                Console.WriteLine("The latest point was detected as an anomaly.");
            }
            else
            {
                Console.WriteLine("The latest point was not detected as an anomaly.");
            }
        }
    }
}