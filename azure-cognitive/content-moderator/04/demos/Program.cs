namespace JobReview
{
    class Program
    {
        static void Main(string[] args)
        {
            Job.SubmitJobAndWaitReview(Consts.FileName, Consts.ContentType, Consts.WorkflowName, Consts.TeamName, Consts.JobOutputFile);
            Review.CreateReviews(Consts.ReviewOutputFile, Consts.ContentType, Consts.TeamName);
        }
    }
}
