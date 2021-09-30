using Microsoft.ProjectOxford.Vision;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        const string API_key = "<< API Key >>";
        const string API_location = "<< API URL >>";

        static void Main(string[] args)
        {
            string imgToAnalyze = @"<< here goes your image path >>";

            SmartThumbnail(imgToAnalyze, 80, 80, true);
            SmartThumbnail(imgToAnalyze, 180, 180, false);

            Console.ReadKey();
        }

        public static void SmartThumbnail(string fname, int width, int height, bool smartCropping)
        {
            Task.Run(async () => {

                string imgname = Path.GetFileName(fname);
                Console.WriteLine($"Thumbnail for image: {imgname}");

                byte[] thumbnail = await SmartThumbnailGeneration(fname, width, height, smartCropping);

                string thumbnaileFullPath = string.Format("{0}\\thumbnail_{1:yyyy-MMM-dd_hh-mm-ss}.jpg",
                    Path.GetDirectoryName(fname), DateTime.Now);

                using (BinaryWriter bw = new BinaryWriter(new FileStream(thumbnaileFullPath, 
                    FileMode.OpenOrCreate, FileAccess.Write)))
                    bw.Write(thumbnail);

            }).Wait();
        }

        public static async Task<byte[]> SmartThumbnailGeneration(string fname, int width, int height, bool smartCropping)
        {
            byte[] thumbnail = null;
            VisionServiceClient client = new VisionServiceClient(API_key, API_location);

            if (File.Exists(fname))
                using (Stream stream = File.OpenRead(fname))
                    thumbnail = await client.GetThumbnailAsync(stream, width, height, smartCropping);

            return thumbnail;
        }
    }
}
