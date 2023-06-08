using Xunit;
using Amazon.Runtime;
using Amazon;

using Amazon.Lambda;
using System.Text.Json;

using System.Text.Json;
using Amazon.DynamoDBv2.Model;
using QueryHealthData.Models;
using System.Diagnostics.Metrics;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;

namespace QueryHealthData.Tests;

public class QueryDataTest
{
    private AWSCredentials credentials = new BasicAWSCredentials("AKIAVOI2WZDF6POOYMEW", "AsUEWttFDBLeXUa++sMOdL2o3zZOwoNDV8esDpas");
    [Fact]
    public async void QueryDocHealthDataLambda()
    {
        var client = new AmazonLambdaClient(credentials, RegionEndpoint.USEast1);

        HealthDocumentQueryInputModel healthDocumentQueryInputModel = new HealthDocumentQueryInputModel();
        healthDocumentQueryInputModel.HashKey = "MRN#100";
        healthDocumentQueryInputModel.Index = "GSI2";

        
        healthDocumentQueryInputModel.SortKey = "ENCOUNTER#120";
     

        var jsonString = JsonSerializer.Serialize(healthDocumentQueryInputModel);

        var request = new Amazon.Lambda.Model.InvokeRequest
        {
            FunctionName = "QueryHealthData",
            Payload = jsonString
        };

        var response = await client.InvokeAsync(request);
        var streamReader = new StreamReader(response.Payload);
        var result = streamReader.ReadToEnd();
        Console.WriteLine(result);
        System.Threading.Thread.Sleep(150);
    }

    [Fact]
    public async void QueryDemoHealthDataLambda()
    {
        var client = new AmazonLambdaClient(credentials, RegionEndpoint.USEast1);

        HealthDocumentQueryInputModel healthDocumentQueryInputModel = new HealthDocumentQueryInputModel();
       
       

        healthDocumentQueryInputModel.HashKey = "2001_09_23";
        healthDocumentQueryInputModel.SortKey = "MRN#0";
      //  healthDocumentQueryInputModel.Index = "GSI2";

        var jsonString = JsonSerializer.Serialize(healthDocumentQueryInputModel);

        var request = new Amazon.Lambda.Model.InvokeRequest
        {
            FunctionName = "QueryHealthData",
            Payload = jsonString
        };

        var response = await client.InvokeAsync(request);
        var streamReader = new StreamReader(response.Payload);
        var result = streamReader.ReadToEnd();
        Console.WriteLine(result);
        System.Threading.Thread.Sleep(150);


    }
}
