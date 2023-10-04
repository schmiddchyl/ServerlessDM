using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Runtime;
using Amazon;
using Amazon.Lambda;
using CreateHealthData.Models;
using System.Text.Json;
using System.Text;
using System.Reflection;

namespace CreateHealthData.Tests;

public class CreateDocuments
{
    //AKIAVOI2WZDFY4CPOOOJ
    private AWSCredentials credentials = new BasicAWSCredentials("AKIAVOI2WZDFY4CPOOOJ", "lgVaRbCF/ZkJwlgqN3XKvlenJybzLkyGqFQ+nqRz");

    [Fact]
    public async void AddDocumentData()
    {
        var client = new AmazonLambdaClient(credentials, RegionEndpoint.USEast1);

        for (int i = 0; i < 1; i++)
        {
            HealthInput input = generateDocuments(i);

            var jsonString = JsonSerializer.Serialize(input);

            var request = new Amazon.Lambda.Model.InvokeRequest
            {
                FunctionName = "CreateHealthData",
                Payload = jsonString
            };

            var response = await client.InvokeAsync(request);
            var streamReader = new StreamReader(response.Payload);
            var result = streamReader.ReadToEnd();
            Console.WriteLine(result);
            System.Threading.Thread.Sleep(75);
        }
    }

    [Fact]
    public async void AddDocuments()
    {
        var client = new AmazonLambdaClient(credentials, RegionEndpoint.USEast1);

        for (int i = 0; i <250; i++)
        {
            HealthInput input = generateDocuments(i);

            var jsonString = JsonSerializer.Serialize(input);

            var request = new Amazon.Lambda.Model.InvokeRequest
            {
                FunctionName = "CreateHealthData",
                Payload = jsonString
            };

            var response = await client.InvokeAsync(request);
            var streamReader = new StreamReader(response.Payload);
            var result = streamReader.ReadToEnd();
            Console.WriteLine(result);
            System.Threading.Thread.Sleep(150);
        }
    }

    private HealthInput generateDocuments(int i)
    {
        String MRN = "MRN#" + ((int)(i / 100)) * 100;
        String Encounter = "ENCOUNTER#"+((int)(i / 10)) * 10;
        DateTime now = DateTime.Now;
        string relevantDate = now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        String restricted = i % 2 == 0 ? "true" : "false";

        HealthData  healthdata = new HealthData();
        healthdata.ItemData = new Dictionary<string, string>();
        long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        healthdata.PK = milliseconds.ToString();
        healthdata.SK = "0";
        healthdata.GSI2PK = MRN;
        healthdata.GSI2SK = Encounter;
        healthdata.ItemData.Add("Bucket", "hcw-img");
        healthdata.ItemData.Add("DocName", "doc_" + i + ".jpg");
        healthdata.ItemData.Add("RelevantDate", relevantDate);
        healthdata.ItemData.Add("restricted", restricted);
        healthdata.ItemData.Add("typeid", "medical-aws-111-2222-333");
        healthdata.ItemData.Add("type-surename", "medical-aws");
        HealthInput input = new HealthInput();
        input.HealthData = healthdata;
        input.path = MRN + "/" + Encounter + "/"  +healthdata.ItemData["DocName"];
        input.content = Base64FromJpg(i);
        return input;
    }


    private string Base64FromJpg (int iValue)
    {
        String fileName = "sample2.jpg";
        if (iValue % 5 == 0 )
        {
            fileName = "sample5.jpg";
        } else if (iValue % 3 == 0)
        {
            fileName = "sample3.jpg";
        }
        else if (iValue % 2 == 0)
        {
            fileName = "sample2.jpg";
        }
        else if (iValue % 4 == 0)
        {
            fileName = "sample4.jpg";
        }
        Console.WriteLine( Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        string filePath = "../../..//testImages/" + fileName;
        byte[] fileData = File.ReadAllBytes(filePath);
        string fileString = Convert.ToBase64String(fileData);
        return fileString;
    }
}
