using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Exceptions
{
    public class InvalidPartitionKeyException : Exception
    {

        public InvalidPartitionKeyException() : base("Partition key cannot be empty.") { }

    }
}
