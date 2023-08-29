using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Lambda;


using Amazon.Runtime;
using Amazon;
using Amazon.DynamoDBv2.DocumentModel;

using System.Text.Json;
using System.Diagnostics.Metrics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using System.IO;
using System.Linq.Expressions;
using System.Collections;
using Amazon.Lambda.APIGatewayEvents;
using System.Text;
using EncountersByPatientSearch.Models;
using EncountersByPatientSearch.Utils;
using Amazon.Lambda.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EncountersByPatientSearch{

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
        _accessKey = System.Environment.GetEnvironmentVariable("AccessKey");
        _secretKey = System.Environment.GetEnvironmentVariable("SecretKey");

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
        List<PatientSearchTerm> terms = new List<PatientSearchTerm>();
        List<HealthDocumentData> overallResult = new List<HealthDocumentData>();
        Console.WriteLine("method start EncountersByPatientSearch");
        if (request.QueryStringParameters != null && request.QueryStringParameters.ContainsKey("filter"))
        {
            String encodedFilter = request.QueryStringParameters["filter"];
            Console.WriteLine("encodedFilter " + encodedFilter);
            var decodedBytes = System.Convert.FromBase64String(encodedFilter);
            String decodedFilter = System.Text.Encoding.UTF8.GetString(decodedBytes);

            terms = JsonSerializer.Deserialize<List<PatientSearchTerm>>(decodedFilter);

            DynamoDBContext dbContext = new DynamoDBContext(_client);

            List<HealthDocumentData> internalResult = null;
            foreach (PatientSearchTerm term in terms)
            {
                Console.WriteLine("term.Name " + term.name);
                if (term.name == "lastName")
                {
                    foreach (String lName in term.value.items)
                    {
                        try
                        {
                            Console.WriteLine("lName " + lName);
                            internalResult = await dbContext.QueryAsync<HealthDocumentData>
                                (
                                    lName,
                                    new DynamoDBOperationConfig
                                    {
                                        OverrideTableName = "HealthData",
                                        IndexName = "GSI3"
                                    }
                                ).GetRemainingAsync();
                            overallResult = overallResult.Concat(internalResult).ToList();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception in queryexecute:" + ex.Message);
                        }
                    }
                }
            }
        }
        PatientSearchResult result = new PatientSearchResult(PatientConverter.ConvertHealthDataToPatient(overallResult));
        overallResult.Clear();
            Console.WriteLine(" pt search result size = " + result.items.Count);
        foreach (Patient patient in result.items)
        {
            try
            {
                DynamoDBContext dbContext = new DynamoDBContext(_client);

                List<HealthDocumentData> internalResult = null;
                Console.WriteLine("patient search for Id " + patient.PatientId);
                internalResult = await dbContext.QueryAsync<HealthDocumentData>
                (
                    patient.PatientId,
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

            IEncounterCollection encounterResult = new EncounterCollection(EncounterConverter.ConvertHealthDataToEncounters(overallResult));

            return new APIGatewayProxyResponse
        {
            StatusCode = 200,
            Body = System.Text.Json.JsonSerializer.Serialize(encounterResult),
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            }
        };
    }

}
}