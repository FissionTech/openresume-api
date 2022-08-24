using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;
using OpenResume.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Services
{
    public class CosmosJSONUploaderService
    {

        private readonly CosmosClient _client;

        private readonly string _dbName;
        private Database? _database;
        private DatabaseProperties? _databaseProperties;

        public CosmosJSONUploaderService(string endpoint, string primaryKey, string dbName)
        {
            this._client = new CosmosClient(endpoint, primaryKey);
            _database = _client.GetDatabase(dbName);

            _dbName = dbName;
        }

        public async Task<bool> InitializeDatabase()
        {
            if (_database is not null && _databaseProperties is not null)
                return true;

            var dbResponse = await _client.CreateDatabaseIfNotExistsAsync(_dbName);

            bool success = dbResponse.StatusCode == System.Net.HttpStatusCode.Created || dbResponse.StatusCode == System.Net.HttpStatusCode.OK;

            if (!success)
                return false;

            _database = dbResponse.Database;
            _databaseProperties = dbResponse.Resource;
            
            return true;
        }

        private async Task<(bool, Container?, ContainerProperties?)> InitializeContainer(string containerName, string partitionKeyPath)
        {
            bool initDbSuccess = await InitializeDatabase();

            if (!initDbSuccess)
                return (false, null, null);

            var containerResponse = await _database!.CreateContainerIfNotExistsAsync(containerName, partitionKeyPath);

            // Todo double check status codes
            bool createContainerSuccess = containerResponse.StatusCode == System.Net.HttpStatusCode.OK || containerResponse.StatusCode == System.Net.HttpStatusCode.Created;

            return (createContainerSuccess, createContainerSuccess ? containerResponse.Container : null, createContainerSuccess ? containerResponse.Resource : null);
        }

        public async Task<bool> TryUploadJSON(string containerName, CosmosStorableJsonSchemaInfo itemInfo, bool overwriteFileIfExists = true)
        {
            bool initDbSuccess = await InitializeDatabase();

            if (!initDbSuccess)
                return false;

            var (createContainerSuccess, container, containerProperties) = await InitializeContainer(containerName, itemInfo.PartitionKeyInfo.PartitionKeyName);

            if (!createContainerSuccess)
                return false;
            
            container.CreateItemAsync(objToUpload, new PartitionKey(obj))
        }

        #region Private Methods
        private async Task<bool> TryCreateContainer()
        {
            new ContainerProperties()
            var containerProps = new ContainerProperties
            {
                Id = _containerName,
                PartitionKeyPath = 
            }
        }
        #endregion
    }
}
