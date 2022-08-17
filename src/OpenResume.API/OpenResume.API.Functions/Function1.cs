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

namespace OpenResume.API.Functions
{
    public static class Function1
    {
        [FunctionName("ResumeUpload")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Triggered resume upload.");

            var (success, resultObj, exception) = await ValidateBody(req);
            if(!success) {
                log.LogError(exception, resultMsg, req)
            }

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        private static async Task<(bool, object, Exception)> ValidateBody(HttpRequest req) {

            // Guard against empty body
            if (req.Body.Length <= 0) {
                string msg = "Resume upload triggered with an empty body.";
                return (false, msg, null);
            }

            using var sr = new StreamReader(req.Body);
            string rawBody = await sr.ReadToEndAsync();

            JObject jsonBody;
            try {
                jsonBody = JObject.Parse(rawBody);
            } catch (JsonReaderException e) {
                var msg = "Request body is not valid JSON";
                 return (false, msg, e);
            }

            return (true, null, null);
        }
    }
}
