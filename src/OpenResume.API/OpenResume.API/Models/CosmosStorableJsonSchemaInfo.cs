using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using OpenResume.API.Exceptions;
using OpenResume.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Models
{
    public class CosmosStorableJsonSchemaInfo<T>
    {

        public readonly JSchema Schema;

        public readonly PartitionKeyInfo PartitionKeyInfo;

        public CosmosStorableJsonSchemaInfo(string jsonSchemaString, PartitionKeyInfo partitionKeyInfo)
        {

            // Attempt to parse JSON Schema
            Schema = JSchema.Parse(jsonSchemaString);
            Schema.Valid = true;

            if ( Schema.Valid is null || !((bool) Schema.Valid) )
                throw new JSchemaException(
                    $"Provided Schema {jsonSchemaString} is not valid.");

            PartitionKeyInfo = partitionKeyInfo;
        }

        public bool TryValidate(string json)
        {
            JToken? parsedToken = null;
            try
            {
                parsedToken = JToken.Parse(json);
            } catch { return false; }

            return (parsedToken is null ? false : TryValidate(parsedToken));
        }

        public bool TryValidate(JToken token)
        {
            IList<ValidationError> errors;

            if (token.TryGetKey(PartitionKeyInfo.PartitionKeyName, out JToken? partitionKeyValueToken))
            {
                if (partitionKeyValueToken is null || partitionKeyValueToken.Type != JTokenType.String)
                    return false;

                bool success = token.IsValid(Schema, out errors);
                return success;
            }
            
            return false;
        }
    }
}
