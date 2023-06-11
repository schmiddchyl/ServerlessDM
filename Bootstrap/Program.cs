using System;
using System.Collections.Generic;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System.Configuration;


namespace ServerlessDM.bootstrap
{
    class Program
    {
        private static AmazonDynamoDBClient client;
        static readonly string TABLE_NAME = "HealthData";
        static readonly string HASH_KEY_NAME = "PK";
        static readonly string HASH_KEY_TYPE = "S";

        static readonly string RANGE_KEY_NAME = "SK";
        static readonly string RANGE_KEY_TYPE = "S";

        static readonly string SECONDARY_INDEX_NAME = "GSI2";
        static readonly string SECONDARY_INDEX_HASH_KEY_NAME = "GSI2PK";
        static readonly string SECONDARY_INDEX_HASH_KEY_TYPE = "S";
        static readonly string SECONDARY_INDEX_RANGE_KEY_NAME = "GSI2SK";
        static readonly string SECONDARY_INDEX_RANGE_KEY_TYPE = "S";

        static readonly string THIRD_INDEX_NAME = "GSI3";
        static readonly string THIRD_INDEX_HASH_KEY_NAME = "GSI3PK";
        static readonly string THIRD_INDEX_HASH_KEY_TYPE = "S";
        static readonly string THIRD_INDEX_RANGE_KEY_NAME = "GSI3SK";
        static readonly string THIRD_INDEX_RANGE_KEY_TYPE = "S";



        static Program()
        {
            AmazonDynamoDBConfig config = new AmazonDynamoDBConfig();
            config.RegionEndpoint = RegionEndpoint.USEast1;

            //NOTE: Change this with your Access key and Secret key that has Dynamo DB permission to create and do CRUD operations to table
            //      This is for DEMO ONLY, secret and access keys SHOULD NOT BE HARDCODED IN THE CODE. 
            //      We need this to build the tables and populate it with demo data.
            AWSCredentials credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["awspub"]
               , ConfigurationManager.AppSettings["awspriv"]);


            client = new AmazonDynamoDBClient(credentials, config);
        }

        static void Main(string[] args)
        {
            try
            {
                /*
                This a build script that will create demo dynamodb tables and populate it with demo data.
                Before running this script make sure credentials is supplied above and Clean-up section is commented.  
                 */


                #region > Create Tables

                CreateHealthDataTable();
                GetTableInformation(TABLE_NAME);

                ListMyTables();

                #endregion


                #region > Seed Data. Populatest the table with demo data.

                //Populates Customer table in command table
              //  SeedHealthData();

                #endregion

                #region > Clean Up. Uncomment this to drop all the tables. Before running comment all code found in Create Table and Seed Data region above.

                //DeleteExampleTable("cqrses-customer-cmd");
                #endregion


                Console.WriteLine("To continue, press Enter");
                Console.ReadLine();
            }
            catch (AmazonDynamoDBException e) { Console.WriteLine(e.Message); }
            catch (AmazonServiceException e) { Console.WriteLine(e.Message); }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        private static void CreateHealthDataTable()
        {
            string tableName = "HealthData";
            Console.WriteLine("\n*** Creating table " + tableName + " ***");

            var request = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = HASH_KEY_NAME,
                        AttributeType = HASH_KEY_TYPE
                    },
                    new AttributeDefinition
                    {
                        AttributeName = RANGE_KEY_NAME,
                        AttributeType = RANGE_KEY_TYPE
                    },
                    new AttributeDefinition
                {
                    AttributeName = SECONDARY_INDEX_HASH_KEY_NAME,
                    AttributeType = SECONDARY_INDEX_HASH_KEY_TYPE
                },
                new AttributeDefinition
                {
                    AttributeName = SECONDARY_INDEX_RANGE_KEY_NAME,
                    AttributeType = SECONDARY_INDEX_RANGE_KEY_TYPE
                },
                               new AttributeDefinition
                {
                    AttributeName = THIRD_INDEX_HASH_KEY_NAME,
                    AttributeType = THIRD_INDEX_HASH_KEY_TYPE
                },
                new AttributeDefinition
                {
                    AttributeName = THIRD_INDEX_RANGE_KEY_NAME,
                    AttributeType = THIRD_INDEX_RANGE_KEY_TYPE
                }

                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = HASH_KEY_NAME,
                        KeyType = "HASH" //Partition key
                    },
                    new KeySchemaElement
                    {
                        AttributeName = RANGE_KEY_NAME,
                        KeyType = "RANGE" //Partition key
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 6
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
            {
                new GlobalSecondaryIndex
                {
                    IndexName = SECONDARY_INDEX_NAME,
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = SECONDARY_INDEX_HASH_KEY_NAME,
                            KeyType = KeyType.HASH
                        },
                        new KeySchemaElement
                        {
                            AttributeName = SECONDARY_INDEX_RANGE_KEY_NAME,
                            KeyType = KeyType.RANGE
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    },
                    Projection = new Projection
                    {
                        ProjectionType = ProjectionType.ALL
                    }
                },                 new GlobalSecondaryIndex
                {
                    IndexName = THIRD_INDEX_NAME,
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = THIRD_INDEX_HASH_KEY_NAME,
                            KeyType = KeyType.HASH
                        },
                        new KeySchemaElement
                        {
                            AttributeName = THIRD_INDEX_RANGE_KEY_NAME,
                            KeyType = KeyType.RANGE
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    },
                    Projection = new Projection
                    {
                        ProjectionType = ProjectionType.ALL
                    }
                }
            },

                TableName = tableName
            };

            var response = client.CreateTableAsync(request).Result;

            var tableDescription = response.TableDescription;
            Console.WriteLine("{1}: {0} \t ReadsPerSec: {2} \t WritesPerSec: {3}",
                      tableDescription.TableStatus,
                      tableDescription.TableName,
                      tableDescription.ProvisionedThroughput.ReadCapacityUnits,
                      tableDescription.ProvisionedThroughput.WriteCapacityUnits);

            //response.TableDescription.LatestStreamArn

            string status = tableDescription.TableStatus;
            Console.WriteLine(tableName + " - " + status);

            WaitUntilTableReady(tableName);
        }

        private static void GetTableInformation(string tableName)
        {
            Console.WriteLine("\n*** Retrieving table information ***");
            var request = new DescribeTableRequest
            {
                TableName = tableName
            };

            var response = client.DescribeTableAsync(request).Result;

            TableDescription description = response.Table;
            Console.WriteLine("Name: {0}", description.TableName);
            Console.WriteLine("# of items: {0}", description.ItemCount);
            Console.WriteLine("Provision Throughput (reads/sec): {0}",
                      description.ProvisionedThroughput.ReadCapacityUnits);
            Console.WriteLine("Provision Throughput (writes/sec): {0}",
                      description.ProvisionedThroughput.WriteCapacityUnits);
        }
        private static void ListMyTables()
        {
            Console.WriteLine("\n*** listing tables ***");
            string lastTableNameEvaluated = null;
            do
            {
                var request = new ListTablesRequest
                {
                    Limit = 2,
                    ExclusiveStartTableName = lastTableNameEvaluated
                };

                var response = client.ListTablesAsync(request).Result;
                foreach (string name in response.TableNames)
                    Console.WriteLine(name);

                lastTableNameEvaluated = response.LastEvaluatedTableName;
            } while (lastTableNameEvaluated != null);
        }

   
        private static void WaitUntilTableReady(string tableName)
        {
            string status = null;
            // Let us wait until table is created. Call DescribeTable.
            do
            {
                System.Threading.Thread.Sleep(5000); // Wait 5 seconds.
                try
                {
                    var res = client.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    }).Result;

                    Console.WriteLine("Table name: {0}, status: {1}",
                              res.Table.TableName,
                              res.Table.TableStatus);
                    status = res.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {
                    // DescribeTable is eventually consistent. So you might
                    // get resource not found. So we handle the potential exception.
                }
            } while (status != "ACTIVE");
        }

    } // end ns 


}