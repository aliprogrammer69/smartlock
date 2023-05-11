using Microsoft.EntityFrameworkCore;

using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Infrastructure.DataBase {
    public sealed class UnitOfWork : IUnitOfWork {
        private readonly SmartLockDbContext _dbContext;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public UnitOfWork(SmartLockDbContext dbContext,
                          IDomainEventsDispatcher domainEventsDispatcher) {
            _dbContext = dbContext;
            _domainEventsDispatcher = domainEventsDispatcher;
        }



        public async Task<int> CommitAsync(bool dispacheEvent = true, CancellationToken cancellationToken = default) {
            ApplyLastUpdateDate();
            if (dispacheEvent)
                await _domainEventsDispatcher.DispatchEventsAsync();
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        #region Private Methods
        private void ApplyLastUpdateDate() {
            var entities = _dbContext.ChangeTracker.Entries<TimeAwareEntity>()
                                    .Where(e => e.State is EntityState.Modified);
            foreach (var entity in entities) {
                entity.Entity.LastUpdateDate = DateTime.Now;
            }
        }
        #endregion
    }
}
