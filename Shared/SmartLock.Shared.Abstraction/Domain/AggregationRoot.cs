namespace SmartLock.Shared.Abstraction.Domain {
    public abstract class AggregationRoot<T> : BaseEntity<T>, IAggregationRoot {
        private readonly List<IDomainEvent> _events = new();
        public IEnumerable<IDomainEvent> Events => _events;

        protected void AddEvent(IDomainEvent domainEvent) =>
            _events.Add(domainEvent);

        public void ClearEvents() => _events.Clear();

    }
}
