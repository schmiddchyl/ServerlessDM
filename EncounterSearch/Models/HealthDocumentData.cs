﻿
using Amazon.DynamoDBv2.DataModel;


namespace ItemSearch.Models
{
    [DynamoDBTable("HealthData")]
    public class HealthDocumentData
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

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string? GSI4PK { get; set; }

        [DynamoDBGlobalSecondaryIndexRangeKey]
        public string? GSI4SK { get; set; }


        [DynamoDBGlobalSecondaryIndexHashKey]
        public string? GSI5PK { get; set; }

        [DynamoDBGlobalSecondaryIndexRangeKey]
        public string? GSI5SK { get; set; }



        [DynamoDBProperty(Converter = typeof(InnerMapOrStringConverter))]
        public Dictionary<string, object> ItemData { get; set; }
        //   public string DocumentContent { get; set; }

    }
}
