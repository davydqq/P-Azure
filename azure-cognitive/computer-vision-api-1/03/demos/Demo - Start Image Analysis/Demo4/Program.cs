﻿using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        const string API_key = "<< API Key >>";
        const string API_location = "<< API URL Location >>";

        static void Main(string[] args)
        {
            string imgToAnalyze = @"<< Image Path >>";

            SmartProcessingImageShowResults(imgToAnalyze, "analyze");
            SmartProcessingImageShowResults(imgToAnalyze, "describe");
            SmartProcessingImageShowResults(imgToAnalyze, "tag");

            Console.ReadKey();
        }

        public static IEnumerable<VisualFeature> GetVisualFeatures()
        {
            return new VisualFeature[]
            {
                VisualFeature.Adult,
                VisualFeature.Categories,
                VisualFeature.Color,
                VisualFeature.Description,
                VisualFeature.Faces,
                VisualFeature.ImageType,
                VisualFeature.Tags
            };
        }

        public static void SmartProcessingImageShowResults(string fname, string method)
        {
            Task.Run(async () => {

                string imgname = Path.GetFileName(fname);
                Console.WriteLine($"Checking image: {imgname}");

                AnalysisResult analyzed = await SmartImageProcessing(fname, method);

                switch (method)
                {
                    case "analyze":
                        ShowResults(analyzed, analyzed.Categories, "Categories");
                        ShowFaces(analyzed);
                        break;

                    case "describe":
                        ShowCaptions(analyzed);
                        break;

                    case "tag":
                        ShowTags(analyzed, 0.9);
                        break;
                }

            }).Wait();
        }

        public static async Task<AnalysisResult> SmartImageProcessing(string fname, string method)
        {
            AnalysisResult analyzed = null;
            VisionServiceClient client = new VisionServiceClient(API_key, API_location);

            IEnumerable<VisualFeature> visualFeatures = GetVisualFeatures();

            if (File.Exists(fname))
                using (Stream stream = File.OpenRead(fname))
                    switch (method)
                    {
                        case "analyze":
                            analyzed = await client.AnalyzeImageAsync(stream, visualFeatures);
                            break;

                        case "describe":
                            analyzed = await client.DescribeAsync(stream);
                            break;

                        case "tag":
                            analyzed = await client.GetTagsAsync(stream);
                            break;
                    }

            return analyzed;
        }

        public static void ShowCaptions(AnalysisResult analyzed)
        {
            var captions = from caption in analyzed.Description.Captions select caption.Text + " - " + caption.Confidence;

            if (captions.Count() > 0)
            {
                Console.WriteLine("Captions >>>>");
                Console.WriteLine(string.Join(", ", captions));
            }
        }

        public static void ShowResults(AnalysisResult analyzed, NameScorePair[] nps, string ResName)
        {
            var results = from result in nps select result.Name + " - " + result.Score.ToString();

            if (results.Count() > 0)
            {
                Console.WriteLine($"{ResName} >>>>");
                Console.WriteLine(string.Join(", ", results));
            }
        }

        public static void ShowFaces(AnalysisResult analyzed)
        {
            var faces = from face in analyzed.Faces select face.Gender + " - " + face.Age.ToString();

            if (faces.Count() > 0)
            {
                Console.WriteLine("Faces >>>>");
                Console.WriteLine(string.Join(", ", faces));
            }
        }

        public static void ShowTags(AnalysisResult analyzed, double confidence)
        {
            var tags = from tag in analyzed.Tags where tag.Confidence > confidence select tag.Name;

            if (tags.Count() > 0)
            {
                Console.WriteLine("Tags >>>>");
                Console.WriteLine(string.Join(", ", tags));
            }
        }
    }
}
