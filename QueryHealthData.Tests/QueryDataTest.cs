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
using System.Configuration;

namespace QueryHealthData.Tests;

public class QueryDataTest
{
    AWSCredentials credentials = new BasicAWSCredentials(
        Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
        Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")
    );
    [Fact]
    public async void QueryDocHealthDataLambda()
    {
        var client = new AmazonLambdaClient(credentials, RegionEndpoint.USEast1);

        HealthDocumentQueryInputModel healthDocumentQueryInputModel = new HealthDocumentQueryInputModel();
        healthDocumentQueryInputModel.HashKey = "MRN#100";
        healthDocumentQueryInputModel.Index = "GSI2";

        var privvv = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
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

    [Fact]
    public async void QueryPatientHealthData()
    {
        var client = new AmazonLambdaClient(credentials, RegionEndpoint.USEast1);

        HealthDocumentQueryInputModel healthDocumentQueryInputModel = new HealthDocumentQueryInputModel();
        healthDocumentQueryInputModel.HashKey = "PATIENT";
        healthDocumentQueryInputModel.Index = "GSI2";
        healthDocumentQueryInputModel.QueryType = "MRN_LIST";

        var privvv = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        healthDocumentQueryInputModel.SortKey = "PATIENT";


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
