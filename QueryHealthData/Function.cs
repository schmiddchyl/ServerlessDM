using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using Amazon;
using QueryHealthData.Models;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3.Model;
using System.Diagnostics.Metrics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using System.IO;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace QueryHealthData
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

        /*
        public async Task<String> FunctionHandler(
            HealthDocumentQueryInputModel queryInput, ILambdaContext context) {  return "ok";
        }*/


        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
       
        public async Task<List<HealthDocumentDataQuery>> FunctionHandler(
            HealthDocumentQueryInputModel queryInput, ILambdaContext context)
        {
            //When run in AWS this will be logged in Cloud Watch
            //Console.WriteLine("Started executing Function {0}", context.FunctionName);
         
            var sortKeyValues = new List<object>();
            string[] tokens = queryInput.SortKey.Split("||");
            foreach (String value in tokens)
            {
                sortKeyValues.Add(value);
            }
            DynamoDBContext dbContext = new DynamoDBContext(_client);

            List<HealthDocumentDataQuery> result = null;
            if (queryInput.QueryType == "MRN_LIST")
            {
                try
                {
                    result = await dbContext.QueryAsync<HealthDocumentDataQuery>
                    (
                        queryInput.HashKey,
                        QueryOperator.BeginsWith,
                        sortKeyValues,
                        new DynamoDBOperationConfig
                        {
                            OverrideTableName = "HealthData",
                            IndexName = queryInput.Index
                        }
                    ).GetRemainingAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in queryexecute:" + ex.Message);
                }

            }
            else
            {
                
                try
                {
                    result = await dbContext.QueryAsync<HealthDocumentDataQuery>
                    (
                        queryInput.HashKey,
                        QueryOperator.Equal,
                        sortKeyValues,
                        new DynamoDBOperationConfig
                        {
                            OverrideTableName = "HealthData",
                            IndexName = queryInput.Index
                        }
                    ).GetRemainingAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in queryexecute:" + ex.Message);
                }
                
            }
           
     
            return result;
        }

        

        private async System.Threading.Tasks.Task<string> ReadFromS3Async(string bucketName, string path, string docName)
        {
            Console.WriteLine("Readfroms3, bucketname: " + bucketName + ", docname: " + docName);
            String doccontent = String.Empty;
            try
            {
                // string decodedString = Encoding.UTF8.GetString(data);
                using (var client = new Amazon.S3.AmazonS3Client(Amazon.RegionEndpoint.USEast1))
                {
                    GetObjectRequest request = new GetObjectRequest();
                    request.BucketName = bucketName + "-resized" + path;
                    request.Key = "resized-" + docName;
                    //  request.BucketName = bucketName;
                    //request.Key =  docName;


                    var response = await client.GetObjectAsync(request);
                    StreamReader reader = new StreamReader(response.ResponseStream);

                    doccontent = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GETS3Object:"
                    + " bucketName: " + bucketName
                    + " path: " + path
                    + " key: " + docName
                    + ex.Message);
            }
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(doccontent);
            return System.Convert.ToBase64String(plainTextBytes);

        }
    }

    /*
      public async Task<List<Document>> QueryTableAsync(string tableName, string partitionKey, List<string> partitionValues, string sortKey = null, string sortValue = null)
    {
        var client = new AmazonDynamoDBClient();

        var partitionKeyValues = partitionValues.Select(pv => new AttributeValue { S = pv }).ToList();

        var queryRequest = new QueryRequest
        {
            TableName = tableName,
            KeyConditionExpression = $"{partitionKey} IN ({string.Join(",", partitionValues.Select((_, i) => $":pk{i}"))})",
            ExpressionAttributeValues = partitionValues.Select((pv, i) => new KeyValuePair<string, AttributeValue>($":pk{i}", new AttributeValue { S = pv })).ToDictionary(kv => kv.Key, kv => kv.Value),
            FilterExpression = "#sortKey = :sortValue",
            ExpressionAttributeNames = new Dictionary<string, string> { { "#sortKey", sortKey } },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":sortValue", new AttributeValue { S = sortValue } } }
        };

        var response = await client.QueryAsync(queryRequest);

        return response.Items.Select(item => Document.FromAttributeMap(item)).ToList();
    }
    */
    // old code for getting bucket along with data
    // revamped strategy to retrieve that in a second call via the api gateway
    /*
    foreach (HealthDocumentDataQuery i in result)
    {
        if (i.ItemData.ContainsKey("Bucket"))
        {
            string bucketName = i.ItemData["Bucket"];
            string path = "";
            string docName = "";

            if (i.ItemData.ContainsKey("DocName"))
            {
                docName = i.ItemData["DocName"];
            }

            //    i.DocumentContent = await ReadFromS3Async(bucketName, path, docName);                    
        }
        Console.WriteLine(i);
    }
    */
}