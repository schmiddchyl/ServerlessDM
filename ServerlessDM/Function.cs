using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using Amazon;
using CommandCreateHealthDocumentLambda.Models;
using CreateHealthDocumentLambda.Models;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CreateHealthDocument
{

    public class Function
    {

        private static AmazonDynamoDBClient _client;
        private readonly string _accessKey;
        private readonly string _secretKey;

        /// <summary>
        /// For testing use only.
        /// </summary>
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

        private void Initialize()
        {
            _client = new AmazonDynamoDBClient(
                new BasicAWSCredentials(_accessKey, _secretKey),
                new AmazonDynamoDBConfig
                {
                    //ServiceURL = _serviceUrl,
                    RegionEndpoint = RegionEndpoint.USEast1
                });
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(HealthDocumentInput input, ILambdaContext context)
        {
            //When run in AWS this will be logged in Cloud Watch
            //       Console.WriteLine("Started executing Function {0}", context.FunctionName);

            DynamoDBContext dbContext = new DynamoDBContext(_client);
            await dbContext.SaveAsync<HealthDocumentData>(input.HealthDocumentData);
            string bucketName = input.HealthDocumentData.ItemData["Bucket"];
            await WriteToS3Async(bucketName, input.path, input.content);
            //When run in AWS this will be logged in Cloud Watch
            //     Console.WriteLine("Finish executing Function {0} @ {1}", context.FunctionName, DateTime.Now);
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
              //  Console.WriteLine("Exception in PutS3Object:" + ex.Message);
                return false;
            }
        }
    }
}

