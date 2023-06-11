using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryHealthData.Models
{
    public class HealthDocumentQueryInputModel

    {
        public string Index { get; set; }

        public string HashKey { get; set; }

        public String SortKey { get; set; }

        public String QueryType { get; set; }   
    }
}
