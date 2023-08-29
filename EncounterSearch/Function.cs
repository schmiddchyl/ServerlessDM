using Amazon.Lambda.Core;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Xml.Linq;
using Amazon.Runtime;
using Amazon;

using EncounterSearch.Utils;
using ItemSearch.Models;



// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ItemSearch;

public class Function
{
    private static AmazonDynamoDBClient? _client;
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
    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
     
        var proxyValue = "";
        if(request.PathParameters != null && request.PathParameters.ContainsKey("proxy"))
        {
            var count = request.PathParameters.Count;
            Console.WriteLine("---------------------------------------");
            proxyValue = request.PathParameters["proxy"] ;
            Console.WriteLine("Setting proxy value to " + proxyValue);
            Console.WriteLine("---------------------------------------");
        } else
        {
            Console.WriteLine("PathParmetersNull or not contains proxy key");
        }
        string[] tokens = proxyValue.Split('/');
        List<HealthDocumentData> overallResult = new List<HealthDocumentData>();
        Console.WriteLine("tokens 0 = " + tokens[0]);
        if (tokens[1] == "encounters")
        {
            string patientId = "MRN#" + tokens[0];

            try
            {
                DynamoDBContext dbContext = new DynamoDBContext(_client);
            
                List<HealthDocumentData> internalResult = null;
                Console.WriteLine("patientId " + patientId);
                internalResult = await dbContext.QueryAsync<HealthDocumentData>
                (
                        patientId,
                        new DynamoDBOperationConfig
                        {
                            OverrideTableName = "HealthData",
                            IndexName = "GSI3"
                        }
                    ).GetRemainingAsync();

                Console.WriteLine("internalResult result returned size" + internalResult.Count());

                Console.WriteLine("result returned");

                overallResult = overallResult.Concat(internalResult).ToList();
            }
            catch (Exception ex)
            {
               
                Console.WriteLine("Exception in queryexecute:" + ex.Message + " " + ex.StackTrace);
            }
        }
       
        IEncounterCollection result = new EncounterCollection(EncounterConverter.ConvertHealthDataToEncounters(overallResult));
        

        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
            Body = System.Text.Json.JsonSerializer.Serialize(result),
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            }
        };
    }

}
