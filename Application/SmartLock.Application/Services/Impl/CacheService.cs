using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

namespace SmartLock.Application.Services.Impl
{
    public sealed class CacheService : IMemoryCache
    {
        public ICacheEntry CreateEntry(object key)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(object key, out object value)
        {
            throw new NotImplementedException();
        }
    }
}
