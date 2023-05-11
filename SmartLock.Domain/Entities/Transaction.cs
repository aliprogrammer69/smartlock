using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Entities {
    public sealed class Transaction : AggregationRoot<ulong> {
        public Transaction(Lock @lock, User user, ActionType actionType, bool success) {
            Lock = @lock;
            LockId = @lock.Id;
            User = user;
            UserId = user.Id;
            Action = actionType;
            Success = success;

        }

        private Transaction() {
        }

        public Lock Lock { get; private set; }
        public ulong LockId { get; private set; }
        public User User { get; private set; }
        public ulong UserId { get; private set; }
        public ActionType Action { get; private set; }
        public bool Success { get; private set; }
    }
}
