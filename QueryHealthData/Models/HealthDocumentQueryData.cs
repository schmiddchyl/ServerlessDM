
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryHealthData.Models
{
    [DynamoDBTable("HealthData")]
    public class HealthDocumentDataQuery
    {
        [DynamoDBHashKey]
        public string PK { get; set; }

        [DynamoDBRangeKey]
        public string SK { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string? GSI2PK { get; set; }

        [DynamoDBGlobalSecondaryIndexRangeKey]
        public string GSI2SK { get; set; }


        [DynamoDBGlobalSecondaryIndexHashKey]
        public string? GSI3PK { get; set; }

        [DynamoDBGlobalSecondaryIndexRangeKey]
        public string GSI3SK { get; set; }

        public Dictionary<string, string> ItemData { get; set; }

     //   public string DocumentContent { get; set; }

    }
}
