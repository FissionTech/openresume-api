using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;
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
        private readonly Database _database;

        private readonly string _containerName;
        private readonly Container _container;

        public CosmosJSONUploaderService(string endpoint, string primaryKey, string dbName, string containerName)
        {
            this._client = new CosmosClient(endpoint, primaryKey);
            _database = _client.GetDatabase(dbName);

            _containerName = containerName;
            _container = _database.GetContainer(containerName);
        }

        //public async Task<bool> TryUploadJSON(string name, JObject objToUpload, bool overwrite = true)
        //{

        //}

        //#region Private Methods
        //private async Task<bool> TryCreateContainer()
        //{
        //    new ContainerProperties()
        //    var containerProps = new ContainerProperties
        //    {
        //        Id = _containerName,
        //        PartitionKeyPath = ""
        //    };
        //}
        //#endregion
    }
}
