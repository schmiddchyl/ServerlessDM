using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using System.Collections.Generic;
using Newtonsoft.Json;
using Amazon.Runtime;
using Amazon;

namespace BucketProcessingUtilities
{


    class Program
    {
        private const string BUCKET_NAME = "hcw-img";
        private const string LAMBDA_FUNCTION_NAME = "YOUR_LAMBDA_FUNCTION_NAME";

       
        private AWSCredentials credentials = new BasicAWSCredentials("", "");

        public async void AddDocumentData()
        {
            var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);

            var lambdaClient = new AmazonLambdaClient(credentials, RegionEndpoint.USEast1);

            var listObjectsRequest = new ListObjectsV2Request
            {
                BucketName = BUCKET_NAME
            };

            ListObjectsV2Response listObjectsResponse;
            do
            {
                listObjectsResponse = await s3Client.ListObjectsV2Async(listObjectsRequest);
                foreach (var s3Object in listObjectsResponse.S3Objects)
                {
                    var payload = new
                    {
                        Records = new List<object>
                    {
                        new
                        {
                            s3 = new
                            {
                                bucket = new
                                {
                                    name = BUCKET_NAME
                                },
                                @object = new
                                {
                                    key = s3Object.Key
                                }
                            }
                        }
                    }
                    };

                    var invokeRequest = new InvokeRequest
                    {
                        FunctionName = LAMBDA_FUNCTION_NAME,
                        InvocationType = "Event",  // Asynchronous invocation
                        Payload = JsonConvert.SerializeObject(payload)
                    };

                    lambdaClient.InvokeAsync(invokeRequest);
                }

                listObjectsRequest.ContinuationToken = listObjectsResponse.NextContinuationToken;

            } while (listObjectsResponse.IsTruncated);  // If there are more objects to list, keep going.

            System.Console.WriteLine("Finished triggering Lambda for all objects in the bucket.");
        }
    }
}
