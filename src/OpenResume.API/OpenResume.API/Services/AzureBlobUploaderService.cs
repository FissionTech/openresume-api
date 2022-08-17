using OpenResume.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Services
{
    internal class AzureBlobUploaderService : IUploaderService
    {
        public async Task<(bool success, Exception exception)>
            TryUploadObjectAsync(object objToUpload)
        {
            throw new NotImplementedException();
        }
    }
}
