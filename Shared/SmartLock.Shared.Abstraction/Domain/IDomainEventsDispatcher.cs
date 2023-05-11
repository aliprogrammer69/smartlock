namespace SmartLock.Shared.Abstraction.Domain {
    public interface IDomainEventsDispatcher {
        Task DispatchEventsAsync();
    }
}
