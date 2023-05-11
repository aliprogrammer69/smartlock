namespace SmartLock.Shared.Abstraction.Infrastructure {
    public interface ICacheService {
        T GetValue<T>(string key, string region = null);
        Task<T> GetValueAsync<T>(string key, string region = null);

        void SetValue(string key, object value, TimeSpan? lifeTime = null);
        Task SetValueAsync(string key, object value, TimeSpan? lifeTime = null);

        void SetValue(string key, object value, DateTimeOffset? expireTime = null);
        Task SetValueAsync(string key, object value, DateTimeOffset? expireTime = null);

        void Delete(string key);

        Task DeleteAsync(string key);
    }
}
