namespace SmartLock.Shared.Abstraction.Domain {
    public abstract class TimeAwareEntity {
        public DateTime CreateDate { get; protected set; } = DateTime.Now;
        public DateTime? LastUpdateDate { get; set; }
    }
}
