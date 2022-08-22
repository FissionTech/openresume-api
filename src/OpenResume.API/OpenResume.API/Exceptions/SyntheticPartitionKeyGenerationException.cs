using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Exceptions
{
    public class SyntheticPartitionKeyGenerationException : Exception
    {

        public SyntheticPartitionKeyGenerationException(string message) : base(message)
        { }

        public SyntheticPartitionKeyGenerationException(
            string message, Exception? innerException) : base(message, innerException)
        { }
    }
}
