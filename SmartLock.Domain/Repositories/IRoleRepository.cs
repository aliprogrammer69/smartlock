using SmartLock.Domain.Entities;

namespace SmartLock.Domain.Repositories {
    public interface IRoleRepository {
        Task<IEnumerable<Role>> GetAll();
        Task<IEnumerable<Role>> FindAll(IEnumerable<string> keys);
        Task<Role> GetByKey(string key);
    }
}
