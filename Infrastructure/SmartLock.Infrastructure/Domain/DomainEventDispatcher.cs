using MediatR;

using SmartLock.Infrastructure.DataBase;
using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Infrastructure.Domain {
    public sealed class DomainEventDispatcher : IDomainEventsDispatcher {
        private readonly SmartLockDbContext _dbContext;
        private readonly IMediator _mediator;

        public DomainEventDispatcher(SmartLockDbContext dbContext,
                                     IMediator mediator) {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task DispatchEventsAsync() {
            IEnumerable<IAggregationRoot> aggregationRoots = _dbContext.ChangeTracker.Entries<IAggregationRoot>()
                                                                                               .Where(e => e.Entity.Events?.Any() != false)
                                                                                               .Select(e => e.Entity)
                                                                                               .ToList();
            IEnumerable<Task> eventTasks = aggregationRoots.SelectMany(e => e.Events)
                                                           .Select(async (@event) => {
                                                               await _mediator.Publish(@event);
                                                           });

            await Task.WhenAll(eventTasks);

            foreach (IAggregationRoot item in aggregationRoots)
                item.ClearEvents();
        }
    }
}
