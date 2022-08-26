using Azure.Storage.Blobs;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using OpenResume.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Services
{
    public class SchemaRetrievalService : ISchemaRetrievalService
    {

        private readonly BlobContainerClient blobContainerClient;

        public SchemaRetrievalService(BlobContainerClient blobContainerClient)
        {
            this.blobContainerClient = blobContainerClient;
        }

        public SchemaRetrievalService(string connectionString, string containerName)
        {
            this.blobContainerClient = new BlobContainerClient(connectionString, containerName);
        }

        public JSchema GetSchema(string schemaName, string schemaVersion = "latest")
            => GetSchemaAsync(schemaName, schemaVersion).GetAwaiter().GetResult();

        public async Task<JSchema> GetSchemaAsync(string schemaName, string schemaVersion = "latest")
        {
            BlobClient blobClient = blobContainerClient.GetBlobClient($"{schemaName}-{schemaVersion}.json");

            if (!blobClient.Exists())
                throw new FileNotFoundException($"Could not find {schemaName}-{schemaVersion}.json in container {blobContainerClient.Name}");

            Stream blobStreamm = await blobClient.OpenReadAsync();
            StreamReader reader = new StreamReader(blobStreamm);
            string content = await reader.ReadToEndAsync();

            try
            {
                return JSchema.Parse(content);
            } catch (Exception e)
            {
                throw new JSchemaException($"Found schema file, but could not deserialize.", e);
            }
        }
    }
}
