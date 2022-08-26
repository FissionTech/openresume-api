using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Services.Interfaces
{
    public interface ISchemaRetrievalService
    {
        public JSchema GetSchema(string schemaname, string schemaVersion);

        public Task<JSchema> GetSchemaAsync(string schemaname, string schemaVersion);
    }
}
