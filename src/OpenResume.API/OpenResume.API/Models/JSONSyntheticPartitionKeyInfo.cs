using Newtonsoft.Json.Linq;
using OpenResume.API.Exceptions;
using OpenResume.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Models
{
    public class JSONSyntheticPartitionKeyInfo
    {

        public readonly string PartitionKeyName;

        public readonly Dictionary<string, Func<JToken, string>> SubKeysAndStringConverters;

        public JSONSyntheticPartitionKeyInfo(string partitionKeyName, Dictionary<string, Func<JToken, string>> keyConverterDictionary)
        {
            PartitionKeyName = partitionKeyName;
            SubKeysAndStringConverters = keyConverterDictionary;
        }

        public JSONSyntheticPartitionKeyInfo(string partitionKeyName, ICollection<string> keys)
        {
            PartitionKeyName = partitionKeyName;

            SubKeysAndStringConverters = new Dictionary<string, Func<JToken, string>>();
            foreach(string key in keys)
            {
                Func<JToken, string> keyConverterFunc = token => token.ToString();
                SubKeysAndStringConverters.Add(key, keyConverterFunc);
            }
        }

        public string GenerateSyntheticKey(JToken token)
        {
            StringBuilder syntheticKeyStringBuilder = new StringBuilder();
            foreach(var kvp in SubKeysAndStringConverters)
            {
                if (token.TryGetKey(kvp.Key, out JToken? tokenValue))
                {
                    try
                    {
                        syntheticKeyStringBuilder.Append(kvp.Value(tokenValue!));
                    } catch (Exception e)
                    {
                        throw new SyntheticPartitionKeyGenerationException($"Unable to apply conversion function delegate to value of JToken {kvp.Key}", e);
                    }
                }

                // We do not need to append anything to the synthetic partition key
                // if there is no valid value. The function will throw an error at the
                // end if the entire key is empty.
            }

            string finalKey = syntheticKeyStringBuilder.ToString();
            if (finalKey.IsNullWhitespaceOrEmpty())
                throw new InvalidPartitionKeyException();

            return syntheticKeyStringBuilder.ToString();
        }

    }
}
