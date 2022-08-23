using OpenResume.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Tests.Unit
{
    public class CosmosStorableJsonSchemaInfoTests
    {

        [Fact]
        public void ValidateSucceedsOnValidSchema()
        {
			string schema =
            @"
			{
				""definitions"": {},
				""$schema"": ""http://json-schema.org/draft-07/schema#"", 
				""$id"": ""https://example.com/object1661202498.json"", 
				""title"": ""Root"", 
				""type"": ""object"",
				""required"": [
					""partitionKey"",
					""key1"",
					""key2""
				],
				""properties"": {
					""partitionKey"": {
						""$id"": ""#root/partitionKey"", 
						""title"": ""Partitionkey"", 
						""type"": ""string"",
						""default"": """",

						""examples"": [

							""test""
						]

					},
					""key1"": {
						""$id"": ""#root/key1"", 
						""title"": ""Key1"", 
						""type"": ""string"",
						""default"": """",
						""examples"": [
							""test""
						]
					},
					""key2"": {
						""$id"": ""#root/key2"", 
						""title"": ""Key2"", 
						""type"": ""string"",
						""default"": """",
						""examples"": [
							""test""
						]
					}
				}
			}
            ";


            string jsonObject =
            @"
			{
				""partitionKey"": ""testtest"",
				""key1"": ""test"",
				""key2"": ""test""
			}
			";

			CosmosStorableJsonSchemaInfo schemaInfo = new CosmosStorableJsonSchemaInfo(
				schema, new JSONSyntheticPartitionKeyInfo("partitionKey", new List<string>() { 
					"key1", "key2" 
				}));

			Assert.True(schemaInfo.TryValidate(jsonObject));
		}

    }
}
