using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Util
{
    public class ResumeUploadRequestValidator : IHttpRequestValidator
    {
        public override async Task<bool> IsValid(HttpRequest value) {

            // Ensure that we have a body
            if (value.Body.Length <= 0) return false;

            // Read body and attempt to deserialize
            try {
                using var sr = new StreamReader(value.Body);
                string rawBody = await sr.ReadToEndAsync();
                JObject.Parse(rawBody);
            } catch { return false; }

            return true;
        }

        public override async Task<(bool success, JObject? result, Exception? exception)> TryParse(HttpRequest value) {
            // Ensure that we have a body
            if (value.Body.Length <= 0) return (false, null, null);

            JObject? parsed = null;
            try {
                using var sr = new StreamReader(value.Body);
                string rawBody = await sr.ReadToEndAsync();
                parsed = JObject.Parse(rawBody);
            } catch (Exception e) { return (false, parsed, e); }
            

            return (true, parsed, null);
        }
    }
}
