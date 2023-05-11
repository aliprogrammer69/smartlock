namespace SmartLock.Shared.Abstraction.Domain {
    public interface IAggregationRoot {
        IEnumerable<IDomainEvent> Events { get; }
        void ClearEvents();
    }
}
