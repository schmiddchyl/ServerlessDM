using System.Reflection;
using UploadHCWImage.Models;
using System.Text.Json;
using Amazon.Runtime;
using Amazon;
using Amazon.Lambda;
using Xunit;

namespace UpdloadHCWImage.tests
{
    public class Tests
    {
   
        private AWSCredentials credentials = new BasicAWSCredentials("", "");

        [Fact]
        public async void Test1()
        {
            var client = new AmazonLambdaClient(credentials, RegionEndpoint.USEast1);
            ImageInput input = generateImageInput();
            var jsonString = JsonSerializer.Serialize(input);

            var request = new Amazon.Lambda.Model.InvokeRequest
            {
                FunctionName = "CreateHealthData",
                Payload = jsonString
            };

            var response = await client.InvokeAsync(request);
            var streamReader = new StreamReader(response.Payload);
            var result = streamReader.ReadToEnd();
        }

        private ImageInput generateImageInput()
        {
            ImageInput input = new ImageInput();
            input.content = Base64FromJpg();
            input.bucketName = "hcw-img";
            input.path = "102";
            return input;
        }

        private string Base64FromJpg()
        {
            String fileName = "sample2.jpg";
            Console.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            string filePath = "../../..//testImages/" + fileName;
            byte[] fileData = File.ReadAllBytes(filePath);
            string fileString = Convert.ToBase64String(fileData);
            return fileString;
        }

    }
}