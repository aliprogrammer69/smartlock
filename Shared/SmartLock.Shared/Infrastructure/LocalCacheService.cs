using System.Runtime.Caching;

using SmartLock.Shared.Abstraction.Infrastructure;

namespace SmartLock.Shared.Infrastructure {
    public sealed class LocalCacheService : ICacheService {
        private readonly ObjectCache _cache = MemoryCache.Default;

        public void Delete(string key) {
            _cache.Remove(key);
        }

        public Task DeleteAsync(string key) {
            return Task.FromResult(_cache.Remove(key));
        }

        public T GetValue<T>(string key, string region = default) {
            return (T)_cache.Get(key, region);
        }

        public Task<T> GetValueAsync<T>(string key, string region = default) {
            return Task.FromResult<T>((T)_cache.Get(key, region));
        }

        public void SetValue(string key, object value, TimeSpan? lifeTime = default) {
            if (value == null) {
                _cache.Remove(key);
            }
            else {
                _cache.Set(key, value, lifeTime == null ? DateTimeOffset.MaxValue : DateTimeOffset.Now.AddSeconds(lifeTime.Value.TotalSeconds));
            }
        }

        public void SetValue(string key, object value, DateTimeOffset? expireTime) {
            if (value == null) {
                _cache.Remove(key);
            }
            else {
                _cache.Set(key, value, expireTime ?? DateTimeOffset.MaxValue);
            }
        }

        public Task SetValueAsync(string key, object value, TimeSpan? lifeTime) {
            return Task.Run(() => SetValue(key, value, lifeTime == null ? DateTimeOffset.MaxValue : DateTimeOffset.Now.AddSeconds(lifeTime.Value.TotalSeconds)));
        }

        public Task SetValueAsync(string key, object value, DateTimeOffset? expireTime) {
            return Task.Run(() => SetValue(key, value, expireTime));
        }
    }
}
