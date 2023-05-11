namespace SmartLock.Domain.Repositories {
    public interface IUnitOfWork {
        Task<int> CommitAsync(bool dispacheEvent = true, CancellationToken cancellationToken = default);
    }
}
