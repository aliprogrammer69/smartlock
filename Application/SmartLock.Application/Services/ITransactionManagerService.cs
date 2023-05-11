using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.Services {
    public interface ITransactionManagerService {
        void Add(Entities.Transaction transaction);
    }
}
