using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Services.Interfaces
{
    public interface IUploaderService
    {
        public Task<(bool success, Exception exception)>
            TryUploadObjectAsync(object objToUpload);
    }
}
