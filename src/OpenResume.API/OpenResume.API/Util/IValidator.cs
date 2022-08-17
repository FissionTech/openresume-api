using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenResume.API.Util
{
    internal interface IValidator<TSource, TResult>
    {
        Task<bool> IsValid(TSource value);

        Task<(bool success, TResult? result, Exception? exception)> TryParse(TSource value);
    }
}
