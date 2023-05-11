using SmartLock.Domain.Entities;

namespace SmartLock.Domain.Repositories {
    public interface ILockRepository {
        Task AddAsync(Lock @lock);
        void Remove(Lock @lock);
        void Edit(Lock @lock);
        Task<IEnumerable<Lock>> GetUserLocks(ulong userId);
        Task<Lock> GetAsync(ulong id);
        Task<bool> Exists(ulong lockId);
    }
}
