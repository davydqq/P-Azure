using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceApiSdkDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IFaceServiceClient faceClient = CreateFaceClient();

            var faceAttr = new[] { FaceAttributeType.Emotion, FaceAttributeType.Age };
            var faces = await faceClient.DetectAsync("https://psfaceapicourse.blob.core.windows.net/images/diverse-people.png",
                returnFaceAttributes: faceAttr);
            
            foreach (var face in faces)
            {
                Console.WriteLine("***** FACE *****");
                Console.WriteLine($"T: {face.FaceRectangle.Top}; L: {face.FaceRectangle.Left}; " +
                    $"W: {face.FaceRectangle.Width}; H: {face.FaceRectangle.Height}");
                Console.WriteLine($"Age: {face.FaceAttributes.Age}: Happiness: {face.FaceAttributes.Emotion.Happiness}");
            }


        }

        static IFaceServiceClient CreateFaceClient() => new FaceServiceClient("83a3a60a141a4d438edc31cff54c6f64", 
            "https://eastus.api.cognitive.microsoft.com/face/v1.0");

    }
}
