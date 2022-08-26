using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using OpenResume.API.Services.Interfaces;

namespace OpenResume.API.Functions
{
    public class GetSchemaFunc
    {

        private ISchemaRetrievalService _schemaRetrievalService;

        public GetSchemaFunc(ISchemaRetrievalService schemaRetrievalService)
        {
            _schemaRetrievalService = schemaRetrievalService;
        }

        [FunctionName("GetSchemaFunc")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var schema = await _schemaRetrievalService.GetSchemaAsync("testschema", "2022-08-26");
                return new OkObjectResult(schema);
            } catch
            {
                return new NotFoundResult();
            }
        }
    }
}
