using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OpenResume.API.Services;
using OpenResume.API.Services.Interfaces;
using OpenResume.API.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(OpenResume.API.Functions.Startup))]
namespace OpenResume.API.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(builder.GetContext().ApplicationRootPath)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton<IConfigurationRoot>(config);

            builder.Services.AddSingleton<IValidator<HttpRequest, JObject?>, ResumeUploadRequestValidator>( s => {
                return new ResumeUploadRequestValidator();
            });

            builder.Services.AddSingleton<ISchemaRetrievalService>(src =>
            {
                return new SchemaRetrievalService(config["GLOBAL_RESUME_STORAGE_CONNECTIONSTRING"], config["GLOBAL_RESUME_STORAGE_SCHEMA_CONTAINER"]);
            });

        }
    }
}
