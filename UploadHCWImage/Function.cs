using Amazon.Lambda.Core;

using Amazon.Runtime;
using Amazon;
using UploadHCWImage.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace UploadHCWImage;

public class Function
{

    private readonly string _accessKey;
    private readonly string _secretKey;
    public Function(string accessKey,
    string secretKey)
    {
        _accessKey = accessKey;
        _secretKey = secretKey;

        Initialize();
    }

    public Function()
    {

        //-- get keys and service url from enviornment variable
        _accessKey = Environment.GetEnvironmentVariable("AccessKey");
        _secretKey = Environment.GetEnvironmentVariable("SecretKey");

        Initialize();
    }
    private void Initialize() { }


    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task FunctionHandler(ImageInput input, ILambdaContext context)
    {
        await WriteToS3Async(input.bucketName, input.path, input.content);
    }

    private async System.Threading.Tasks.Task<bool> WriteToS3Async(string bucketName,
    string path, string content)
    {
        try
        {


            byte[] data = Convert.FromBase64String(content);
            using (MemoryStream stream = new MemoryStream(data))
            {
                // string decodedString = Encoding.UTF8.GetString(data);
                using (var client = new Amazon.S3.AmazonS3Client(Amazon.RegionEndpoint.USEast1))
                {
                    var request = new Amazon.S3.Model.PutObjectRequest
                    {
                        ContentType = "image/jpeg",
                        BucketName = bucketName,
                        Key = path,
                        InputStream = stream
                    };
                    var response = await client.PutObjectAsync(request);

                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception in PutS3Object:" + ex.Message);
            return false;
        }
    }


}
