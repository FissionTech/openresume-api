using Newtonsoft.Json.Linq;
using OpenResume.API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenResume.API.Extensions
{
    internal static class JTokenExtensions
    {
        public static bool TryGetKey(this JToken token, string key, out JToken? value)
        {
            value = default(JToken);
            if (token.Type != JTokenType.Object)
                return false;

            value = token.SelectToken(key);
            return value is not null;
        }

        public static bool TryGetKeyAsString<T>(this JToken token, string key, out T? value)
        {
            value = default(T);
            if (token.Type != JTokenType.Object)
                return false;

            JToken? checkedToken;

            if (token.TryGetKey(key, out checkedToken))
            {
                if (checkedToken is null)
                    return false;

                try
                {
                    value = checkedToken!.ToObject<T>();
                    return true;
                }
                catch { return false; }
            }
            else return false;
        }

    }
}
