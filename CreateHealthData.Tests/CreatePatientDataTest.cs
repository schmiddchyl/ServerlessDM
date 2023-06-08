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


namespace CreateHealthData.Tests
{
    public class CreatePatientDemographicData
    {
        private AWSCredentials credentials = new BasicAWSCredentials("AKIAVOI2WZDF6POOYMEW", "AsUEWttFDBLeXUa++sMOdL2o3zZOwoNDV8esDpas");
        [Fact]
        public async void AddDemographicData()
        {
            var client = new AmazonLambdaClient(credentials, RegionEndpoint.USEast1);

            HealthInput input = createPatient("Dan", "Schmidt", 0, "2001_09_23", "false", "123 fake st", "555-555-1212");
            await DoLambda(client, input);

            input = createPatient("Theo", "Schmidt", 0, "2011_04_23", "false", "123 fake st", "555-555-1212");
            await DoLambda(client, input);

            input = createPatient("Brian", "Finch", 0, "1966_03_11", "false", "444 real st", "333-555-1212");
            await DoLambda(client, input);
        }

        private static async Task DoLambda(AmazonLambdaClient client, HealthInput input)
        {
            var jsonString = JsonSerializer.Serialize(input);
            var request = new Amazon.Lambda.Model.InvokeRequest
            {
                FunctionName = "CreateHealthData",
                Payload = jsonString
            };

            var response = await client.InvokeAsync(request);
            var streamReader = new StreamReader(response.Payload);
            var result = streamReader.ReadToEnd();
            Assert.Equal(String.Empty, result);
            Console.WriteLine(result);
        }

      
        private HealthInput createPatient(String firstName, String lastName, int mrn, String dob, String restricted, String address, String phone)
        {
            String MRN = "MRN#" + mrn;
            String DOB = dob;
            DateTime now = DateTime.Now;
            string relevantDate = now.ToString("yyyy-MM-ddTHH:mm:ss.fff");

            HealthData healthdata = new HealthData();
            healthdata.ItemData = new Dictionary<string, string>();
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            healthdata.PK = dob;
            healthdata.SK = MRN;
            healthdata.GSI2PK = MRN;
            healthdata.GSI2SK = dob;
            healthdata.ItemData.Add("FirstName", firstName);
            healthdata.ItemData.Add("LastName", lastName);
            healthdata.ItemData.Add("CreatedDate", relevantDate);
            healthdata.ItemData.Add("restricted", restricted);
            healthdata.ItemData.Add("Address", address);
            healthdata.ItemData.Add("Phone", phone);
           
            HealthInput input = new HealthInput();
            input.HealthData = healthdata;
            return input;
        }

    }// class
} // ns
