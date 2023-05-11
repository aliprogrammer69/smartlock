namespace SmartLock.Shared.Abstraction.Domain {
    public abstract class BaseEntity<T> : TimeAwareEntity {
        public T Id { get; protected set; }
    }
}
