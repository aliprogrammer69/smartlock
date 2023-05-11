using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Infrastructure.DataBase {
    public abstract class AbstractRepository<TEntity> where TEntity : IAggregationRoot {
        protected readonly SmartLockDbContext _dbContext;

        public AbstractRepository(SmartLockDbContext dbContext) {
            _dbContext = dbContext;
        }

        public virtual Task AddAsync(TEntity entity) =>
            _dbContext.AddAsync(entity).AsTask();

        public virtual void Remove(TEntity entity) =>
            _dbContext.Remove(entity);

        public virtual void Edit(TEntity entity) =>
            _dbContext.Update(entity);
    }
}
