using Amazon.DynamoDBv2.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ItemSearch.Models
{
    public class MyMapAttribute
    {
        [DynamoDBProperty]
        [JsonConverter(typeof(InnerMapOrStringConverter))]
        public Dictionary<string, object> ItemData { get; set; }
    }
}
