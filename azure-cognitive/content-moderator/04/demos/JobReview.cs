using Microsoft.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator.Models;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JobReview
{
    public class ReviewItem
    {
        public string Type;
        public string Url;
        public string ContentId;
        public string ReviewId;
    }

    public class Review
    {
        public static void CreateReviews(string outputFile, string mediaType, string teamName)
        {
            string[] ImageUrls = new string[] { Consts.FileName };
            List<ReviewItem> reviewItems = new List<ReviewItem>();

            using (TextWriter writer = new StreamWriter(outputFile, false))
            {
                using (var client = ContentModerator.NewClient())
                {
                    writer.WriteLine("Reviews for the following images: ", true);
                    List<CreateReviewBodyItem> requestInfo = CreateReview(mediaType, ImageUrls, reviewItems, writer);
                    ReviewResponse(teamName, reviewItems, writer, client, requestInfo);

                    Thread.Sleep(Consts.ThrottleRate);
                }

                writer.Flush();
                writer.Close();
            }
        }

        private static List<CreateReviewBodyItem> CreateReview(string mediaType, string[] ImageUrls, List<ReviewItem> reviewItems, TextWriter writer)
        {
            List<CreateReviewBodyItem> requestInfo =
                                    new List<CreateReviewBodyItem>();

            List<CreateReviewBodyItemMetadataItem> metadata =
                new List<CreateReviewBodyItemMetadataItem>(
                    new CreateReviewBodyItemMetadataItem[] {
                        new CreateReviewBodyItemMetadataItem(Consts.MetadataKey, Consts.MetadataValue)
                    }
                );

            for (int i = 0; i < ImageUrls.Length; i++)
            {
                var itemInfo = new ReviewItem()
                {
                    Type = mediaType,
                    ContentId = i.ToString(),
                    Url = ImageUrls[i],
                    ReviewId = null
                };

                writer.WriteLine($" - {itemInfo.Url}; with id = {itemInfo.ContentId}.", true);

                requestInfo.Add(new CreateReviewBodyItem(
                    itemInfo.Type, itemInfo.Url, itemInfo.ContentId,
                    Consts.CallbackEndpoint, metadata));

                reviewItems.Add(itemInfo);
            }

            return requestInfo;
        }

        private static void ReviewResponse(string teamName, List<ReviewItem> reviewItems, TextWriter writer, ContentModeratorClient client, 
            List<CreateReviewBodyItem> requestInfo)
        {
            var reviewResponse = client.Reviews.CreateReviewsWithHttpMessagesAsync(
                                    "application/json", teamName, requestInfo);

            var reviewIds = reviewResponse.Result.Body;
            for (int i = 0; i < reviewIds.Count; i++)
            {
                reviewItems[i].ReviewId = reviewIds[i];
            }

            writer.WriteLine(JsonConvert.SerializeObject(reviewIds, Formatting.Indented));
        }
    }

    public class Job
    {
        public static void SubmitJobAndWaitReview(string fn, string contentType, string workflowName, string teamName, string outputFile)
        {
            using (TextWriter writer = new StreamWriter(outputFile, false))
            {
                using (var client = ContentModerator.NewClient())
                {
                    string jobId = string.Empty;

                    ExecuteJob(client, fn, contentType, workflowName, teamName, writer, out jobId, 
                        out Task<HttpOperationResponse<Microsoft.CognitiveServices.ContentModerator.Models.Job>> jobDetails);

                    WaitForReview();

                    jobDetails = JobResults(teamName, writer, client, jobId);
                }

                writer.Flush();
                writer.Close();
            }
        }


        private static void ExecuteJob(ContentModeratorClient client, string fn, string contentType, string workflowName, string teamName, TextWriter writer, 
            out string jobId,  
            out Task<HttpOperationResponse<Microsoft.CognitiveServices.ContentModerator.Models.Job>> jobDetails)
        {
            writer.WriteLine($"Creating a review job for file: {fn}");
            var content = new Content(fn);

            var jobResult = client.Reviews.CreateJobWithHttpMessagesAsync(
                    teamName, contentType, Consts.ContentId, workflowName, "application/json", content,
                    Consts.CallbackEndpoint);

            jobId = jobResult.Result.Body.JobIdProperty;
            writer.WriteLine(
                JsonConvert.SerializeObject(jobResult.Result.Body, Formatting.Indented));

            Thread.Sleep(2000);
            writer.WriteLine();

            writer.WriteLine("Job status...");
            jobDetails = client.Reviews.GetJobDetailsWithHttpMessagesAsync(
                    teamName, jobId);

            writer.WriteLine(JsonConvert.SerializeObject(
                    jobDetails.Result.Body, Formatting.Indented));
        }

        private static void WaitForReview()
        {
            Console.WriteLine();
            Console.WriteLine("Do the Review using the Review Tool.");
            Console.WriteLine("Once done, press any key to continue.");
            Console.ReadKey();
        }

        private static Task<HttpOperationResponse<Microsoft.CognitiveServices.ContentModerator.Models.Job>> JobResults(string teamName, TextWriter writer, ContentModeratorClient client, string jobId)
        {
            Task<HttpOperationResponse<Microsoft.CognitiveServices.ContentModerator.Models.Job>> jobDetails;

            Console.WriteLine();
            Console.WriteLine($"Waiting {Consts.latencyDelay} seconds for the results...");
            Thread.Sleep(Consts.latencyDelay * 1000);

            writer.WriteLine("Review details.");
            jobDetails = client.Reviews.GetJobDetailsWithHttpMessagesAsync(teamName, jobId);

            writer.WriteLine(
                JsonConvert.SerializeObject(jobDetails.Result.Body, Formatting.Indented));

            return jobDetails;
        }
    }
}
