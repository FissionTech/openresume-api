using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Util
{
    public abstract class IHttpRequestValidator : IValidator<HttpRequest, JObject>
    {
        public abstract Task<bool> IsValid(HttpRequest value);

        public abstract Task<(bool success, JObject? result, Exception? exception)> TryParse(HttpRequest value);
    }
}
