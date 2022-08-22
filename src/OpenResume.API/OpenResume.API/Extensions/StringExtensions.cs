using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNullWhitespaceOrEmpty(this string checkString)
            => string.IsNullOrEmpty(checkString) 
            || string.IsNullOrWhiteSpace(checkString);
    }
}
