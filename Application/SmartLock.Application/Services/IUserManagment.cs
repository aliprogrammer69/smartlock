using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.Services {
    public interface IUserManagment {
        Entities.User CurrentUser { get; }
        Task<bool> Authenticate(ulong userId);
    }
}
