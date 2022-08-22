using Newtonsoft.Json.Linq;
using OpenResume.API.Exceptions;
using OpenResume.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Tests.Unit
{
    public class JSONSyntheticPartitionKeyInfoTests
    {

        [Fact]
        public void GeneratesSimplePartitionKey()
        {
            string sampleJson = 
            @"
            { 
                'key1': 'k1val-',
                'key2': 'k2val'
            }
            ";

            var keys = new List<string>()
            {
                "key1",
                "key2"
            };

            JSONSyntheticPartitionKeyInfo info = new JSONSyntheticPartitionKeyInfo(
                "partitionKey",
                keys);

            string expectedKey = "k1val-k2val";

            JToken parsedToken = JToken.Parse(sampleJson);

            Assert.Equal(expectedKey, info.GenerateSyntheticKey(parsedToken));
        }

        [Fact]
        public void GeneratesComplexPartitionKey()
        {
            string sampleJson =
            @"
            { 
                'key1': 'k1val',
                'key2': [
                    'k2a1',
                    'k2a2'
                ],
                'key3': {
                    'key4': 'k4val',
                    'key5': false
                }
            }
            ";

            var converterDict = new Dictionary<string, Func<Newtonsoft.Json.Linq.JToken, string>>()
            {
                { "key1", jToken => $"{jToken}-" },
                { "key2", jToken =>  ((JArray)jToken).Aggregate((c, n) => $"{c}-{n}").ToString()},
                { "key3", jToken => $"-{jToken["key4"]}-{jToken["key5"]}" }
            };

            JSONSyntheticPartitionKeyInfo info = new JSONSyntheticPartitionKeyInfo(
                "partitionKey",
                converterDict);

            string expectedKey = "k1val-k2a1-k2a2-k4val-False";

            JToken parsedToken = JToken.Parse(sampleJson);

            Assert.Equal(expectedKey, info.GenerateSyntheticKey(parsedToken));
        }

        [Fact]
        public async Task ExceptsOnGenerationError()
        {
            string sampleJson =
            @"
            { 
                'key1': 'k1val',
                'key2': [
                    'k2a1',
                    'k2a2'
                ],
                'key3': {
                    'key4': 'k4val',
                    'key5': false
                }
            }
            ";

            var converterDict = new Dictionary<string, Func<Newtonsoft.Json.Linq.JToken, string>>()
            {
                { "key1", jToken => throw new Exception() },
                { "key2", jToken =>  ((JArray)jToken).Aggregate((c, n) => $"{c}-{n}").ToString()},
                { "key3", jToken => $"-{jToken["key4"]}-{jToken["key5"]}" }
            };

            JSONSyntheticPartitionKeyInfo info = new JSONSyntheticPartitionKeyInfo(
                "partitionKey",
                converterDict);

            string expectedKey = "k1val-k2a1-k2a2-k4val-False";

            JToken parsedToken = JToken.Parse(sampleJson);

            Assert.Throws<SyntheticPartitionKeyGenerationException>(() => info.GenerateSyntheticKey(parsedToken));
        }

        [Fact]
        public async Task ExceptsOnEmptyKey()
        {
            string sampleJson =
            @"
            { 
                'key1': '',
                'key2': ''
            }
            ";

            var converterDict = new Dictionary<string, Func<Newtonsoft.Json.Linq.JToken, string>>()
            {
                { "key1", jToken => jToken.ToString() },
                { "key2", jToken => jToken.ToString() }
            };

            JSONSyntheticPartitionKeyInfo info = new JSONSyntheticPartitionKeyInfo(
                "partitionKey",
                converterDict);

            string expectedKey = "k1val-k2val";

            JToken parsedToken = JToken.Parse(sampleJson);

            Assert.Throws<InvalidPartitionKeyException>(() => info.GenerateSyntheticKey(parsedToken));
        }

    }
}
