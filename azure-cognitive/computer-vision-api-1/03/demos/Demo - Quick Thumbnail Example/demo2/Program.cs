using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Demo
{
    static class Program
    {
        const string subscriptionKey = "<< API Key >>";
        const string uriBase = "<< API URL Location >>/generateThumbnail";

        static void Main()
        {
            string imageFilePath = @"<< Image Path >>";

            GenerateThumbnail(imageFilePath, 80, 80, true);
            Console.ReadLine();
        }

        public static async void GenerateThumbnail(string imageFilePath, int width, int height, bool smart)
        {
            byte[] thumbnail = await GetThumbnail(imageFilePath, width, height, smart);

            string thumbnaileFullPath = string.Format("{0}\\thumbnail_{1:yyyy-MMM-dd_hh-mm-ss}.jpg",
                Path.GetDirectoryName(imageFilePath), DateTime.Now);

            using (BinaryWriter bw = new BinaryWriter(new FileStream(thumbnaileFullPath, FileMode.OpenOrCreate, FileAccess.Write)))
                bw.Write(thumbnail);
        }

        public static async Task<byte[]> GetThumbnail(string imageFilePath, int width, int height, bool smart)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            string requestParameters = $"width={width.ToString()}&height={height.ToString()}&smartCropping={smart.ToString().ToLower()}";
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response = null;
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);

                return await response.Content.ReadAsByteArrayAsync();
            }
        }

        public static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}