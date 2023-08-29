using Amazon.DynamoDBv2.DataModel;
using PatientSearch.Models;
using System.Text;
using System.Text.Json;

namespace PatientSearchTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            List<String> names = new List<String>();
            names.Add("Brooks");
            List<PatientSearchTerm> terms = new List<PatientSearchTerm>();

            PatientSearchTerm term1 = new PatientSearchTerm();
            term1.name = "lastName";
            term1.type = "collection";
            PatientSearchValue pv1 = new PatientSearchValue();
            pv1.items = names;
            term1.value = pv1 ;
            terms.Add(term1);
            string strJson = JsonSerializer.Serialize<List<PatientSearchTerm>>(terms);


            String searchInput = "[{\"name\":\"lastName\", \"type\":\"collection\", \"value\": {\"items\":[\"Brooks\"]}}]";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(searchInput);
            String b64 = System.Convert.ToBase64String(plainTextBytes);
            var fromB64 = System.Convert.FromBase64String(b64);
            String decoded = System.Text.Encoding.UTF8.GetString(fromB64);


            terms = JsonSerializer.Deserialize<List<PatientSearchTerm>>(decoded);
            foreach (PatientSearchTerm term in terms)
            {
                Console.WriteLine("term.name " + term.name);
                if (term.name == "lastName")
                {
                    foreach (String lName in term.value.items)
                        try
                        {
                            Console.WriteLine("lName " + lName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception in queryexecute:" + ex.Message);
                        }
                }
            }

            Assert.Pass();
        }

        [Test]
        public void Test2()
        {

            String searchResponse = "[\r\n    {\r\n        \"PK\":  \"1966_03_11\",\r\n        \"SK\":  \"MRN#0\",\r\n        \"GSI2PK\":  \"MRN#0\",\r\n        \"GSI2SK\":  \"1966_03_11\",\r\n        \"GSI3PK\":  \"Brooks\",\r\n        \"GSI3SK\":  \"Darin\",\r\n        \"ItemData\":  {\r\n                         \"CreatedDate\":  \"2023-08-17T14:06:34.076\",\r\n                         \"LastName\":  \"Finch\",\r\n                         \"Address\":  \"444 real st\",\r\n                         \"FirstName\":  \"Brian\",\r\n                         \"restricted\":  \"false\",\r\n                         \"Phone\":  \"333-555-1212\"\r\n                     }\r\n    },\r\n    {\r\n        \"PK\":  \"1976_03_11\",\r\n        \"SK\":  \"MRN#25\",\r\n        \"GSI2PK\":  \"MRN#25\",\r\n        \"GSI2SK\":  \"1976_03_11\",\r\n        \"GSI3PK\":  \"Brooks\",\r\n        \"GSI3SK\":  \"Fred\",\r\n        \"ItemData\":  {\r\n                         \"CreatedDate\":  \"2023-08-17T14:06:34.076\",\r\n                         \"LastName\":  \"Finch\",\r\n                         \"Address\":  \"444 real st\",\r\n                         \"FirstName\":  \"Brian\",\r\n                         \"Phone\":  \"333-555-1212\",\r\n                         \"restricted\":  \"false\"\r\n                     }\r\n    }\r\n]";


            List<PatientSearchResult> respnse = JsonSerializer.Deserialize<List<PatientSearchResult>>(searchResponse);


            Assert.Pass();
        }
    }




}