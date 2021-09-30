using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator;

namespace JobReview
{
    public class Consts
    {
        #region "Azure specific"
        public static readonly string AzureRegion = "<< Your Azure region >>";
        public static readonly string AzureBaseURL =
            $"https://{AzureRegion}.api.cognitive.microsoft.com";
        public static readonly string CMSubscriptionKey = "<< Your subscription key >>";
        #endregion

        #region "Workflow specific"
        public static readonly string WorkflowName = "default";
        public static readonly string TeamName = "<< Your team name >>";
        public static readonly string ContentType = "Image";
        public static readonly string FileName =
            "https://moderatorsampleimages.blob.core.windows.net/samples/sample3.png";
        #endregion

        #region "Job specific"
        public static readonly string JobOutputFile = @"C:\Temp\JobResult.txt";
        public static readonly int latencyDelay = 5;
        public static readonly string ContentId = "contentID";
        #endregion

        #region "Review specific"
        public static readonly string MetadataKey = "a";
        public static readonly string MetadataValue = "true";
        public static readonly string Subteam = null;
        #endregion

        #region "Job / Review specific"
        public static readonly string ReviewOutputFile = @"C:\Temp\ReviewResult.txt";
        public static readonly int ThrottleRate = 3000;
        public static readonly string CallbackEndpoint = "";
        #endregion
    }

    public static class ContentModerator
    {
        public static ContentModeratorClient NewClient()
        {
            ContentModeratorClient client = new ContentModeratorClient(
                new ApiKeyServiceClientCredentials(Consts.CMSubscriptionKey));

            client.Endpoint = Consts.AzureBaseURL;
            return client;
        }
    }
}
