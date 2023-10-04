using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using Amazon;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using HealtDataBFF.Models;
using HealthDataBFF.Models;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime.Internal.Transform;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HealthDataBFF {


public class Function
{


    private static AmazonDynamoDBClient _client;
    private readonly string _accessKey;
    private readonly string _secretKey;

    public Function(string accessKey,   string secretKey)
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



    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
 
        //Console.WriteLine("method start HealthDataBff");
        //Console.WriteLine("requst body " + request.Body);
        PatientSearchTerm search = JsonSerializer.Deserialize<PatientSearchTerm > (request.Body);
        Console.WriteLine("search mrn = " + search.mrn  );
        DynamoDBContext dbContext = new DynamoDBContext(_client);

        List<HealthDocumentData> result = null;

            if (search.searchType == "DEMOGRAPHICS")
            {
                Console.WriteLine("DEMOGRAPHICS on search mrn = " + search.mrn);
                try
                {
                    result = await dbContext.QueryAsync<HealthDocumentData>
                         (
                            search.mrn,
                            new DynamoDBOperationConfig
                            {
                                OverrideTableName = "HealthData",
                                IndexName = "GSI4"
                            }
                        ).GetRemainingAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in queryexecute:" + ex.Message);
                }
            } else if (search.searchType == "ENCOUNTER")
            {
                Console.WriteLine("Encounter on search mrn = " + search.mrn + " and encounter " + search.encounter);
                try
                {
                    IEnumerable<string> encounter = new[] { search.encounter };
                    result = await dbContext.QueryAsync<HealthDocumentData>
                         (
                            search.mrn,
                            QueryOperator.Equal,
                            encounter,
                            new DynamoDBOperationConfig
                            {
                                OverrideTableName = "HealthData",
                                IndexName = "GSI2"
                            }
                        ).GetRemainingAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in queryEncounter;" + ex.Message);
                }
            }

            string textresult = System.Text.Json.JsonSerializer.Serialize(result);

            Console.WriteLine("textresult:" + textresult);
            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = System.Text.Json.JsonSerializer.Serialize(result),
                Headers = new Dictionary<string, string>
                {
                    {"Access-Control-Allow-Headers", "Content-Type" },
                    {"Access-Control-Allow-Origin", "*" },
                    {"Access-Control-Allow-Methods", "OPTIONS,POST,GET" },
                    { "Content-Type", "application/json" }
                }
            };
    }

    }
}