using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
namespace EncountersByPatientSearch.Models
{

    public class InnerMapOrStringConverter : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            Console.WriteLine("toenctry document");
            if (value is string strVal)
            {
                return new Primitive(strVal);
            }
            else if (value is Dictionary<string, object> dictVal)
            {
                Console.WriteLine("toenctry document dic");
                var document = new Document();
                foreach (var kvp in dictVal)
                {
                    // This assumes that the inner-most items will only be strings. 
                    // If they could be more nested maps, you'd need to recursively handle those here.
                    document[kvp.Key] = new Primitive(kvp.Value.ToString());
                }
                return document;
            }
            throw new ArgumentException("Unsupported type", nameof(value));
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            Console.WriteLine("**********************************************fromEntry - start********");
            
            if (entry is Primitive primitive)
            {
                
                return primitive.AsString();
            }
            else if (entry is Document document)
            {
                var result = new Dictionary<string, object>();
                foreach (var kvp in document)
                {
                    DynamoDBEntry ev = kvp.Value;
                
                    Document innerDoc = ev.AsDocument();
               
                    if (innerDoc != null)
                    {
                        Code code = new Code();
                        code.systemName = innerDoc["systemName"];
                        code.code = innerDoc["code"];
                        string kvikey = ""; string kvivalue = "";
                            string returnValue  = "";
                        foreach (var kvi in innerDoc)
                        {
                            kvikey = kvi.Key; 
                            kvivalue = kvi.Value.ToString();
                            returnValue = "{" + kvikey + ":" + kvivalue + "}";
                         
                        }
                        result[kvp.Key] = code;
                    } else
                    {
          
                        result[kvp.Key] = ev.AsString();
                    
                    }
                }
                return result;
            }
            else
            {

                Console.WriteLine("entery is not document" +
                entry.AsString());
            }
            throw new ArgumentException("Unrecognized DynamoDB entry", nameof(entry));
        }
    }
}