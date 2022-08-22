using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Exceptions
{
    public class BadUriException : Exception
    {
        public BadUriException(UriKind uriKind, string uriString)
            : base($"URI {uriString} is not a well-formed URI string for UriKind {Enum.GetName<UriKind>(uriKind)}")
        { }
    }
}
