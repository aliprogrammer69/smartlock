using SmartLock.Domain.Entities;

namespace SmartLock.Domain.Repositories {
    public interface IUserRepository {
        Task AddAsync(User user);
        void Remove(User user);
        void Edit(User user);
        Task<User> GetByIdAsync(ulong id);
        Task<User> GetByUsernameAsync(string username);
        Task<bool> HasAccessToLock(ulong userId, ulong lockId);
    }
}
