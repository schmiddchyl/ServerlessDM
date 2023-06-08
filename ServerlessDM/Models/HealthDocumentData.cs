using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreateHealthDocumentLambda.Models
{
    [DynamoDBTable("HealthDocuments")]
    public class HealthDocumentData
    {
        [DynamoDBHashKey]
        public string PK { get; set; }


        [DynamoDBRangeKey]
        public string SK { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string? GSI1PK { get; set; }

        [DynamoDBGlobalSecondaryIndexRangeKey]
        public string GSI1SK { get; set; }

        public Dictionary<string, string> ItemData { get; set; }

    }
}
