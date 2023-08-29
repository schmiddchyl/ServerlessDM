using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ItemSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemSearch.Utils
{
    public class DynamoConverter : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {


            DynamoDBEntry entry = new Primitive
            {
                Value = value.ToString()
            };
            return entry;
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            Console.WriteLine("fromEntry");
            Primitive primitive = entry as Primitive;
            if (primitive == null || !(primitive.Value is String) || string.IsNullOrEmpty((string)primitive.Value))
                throw new ArgumentOutOfRangeException();

            string primitiveValue = (string)primitive.Value;
            if (primitiveValue.Contains("{") )
            {
                return System.Text.Json.JsonSerializer.Deserialize<Code>(primitiveValue);

            }
    
            return primitiveValue;
        }
    }
}
