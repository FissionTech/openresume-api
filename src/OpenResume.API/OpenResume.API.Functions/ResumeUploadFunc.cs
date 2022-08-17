using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenResume.API.Util;
using Microsoft.Extensions.Configuration;

namespace OpenResume.API.Functions
{
    public class ResumeUploadFunc
    {

        private readonly IConfiguration _config;
        private readonly IValidator<HttpRequest, JObject?> _requestValidator;

         public ResumeUploadFunc(IConfigurationRoot config, IValidator<HttpRequest, JObject?> requestValidator)
        {
            this._config = config;
            this._requestValidator = requestValidator;
        }

        [FunctionName("ResumeUpload")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Triggered resume upload.");

            var (success, result, exception) = await _requestValidator.TryParse(req);

            if(!success)
                return new BadRequestObjectResult("Unable to deserialize request body to JSON object.");

            return new OkObjectResult("Acceptable request body.");
        }
    }
}
