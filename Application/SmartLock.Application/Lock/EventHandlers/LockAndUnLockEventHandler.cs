using MediatR;

using SmartLock.Application.Services;
using SmartLock.Domain;
using SmartLock.Domain.Events.Lock;

namespace SmartLock.Application.Lock.EventHandlers {
    public sealed class LockAndUnLockEventHandler : INotificationHandler<Unlocked>, INotificationHandler<Locked> {
        private readonly ITransactionManagerService _transactionManager;

        public LockAndUnLockEventHandler(ITransactionManagerService transactionManager) {
            _transactionManager = transactionManager;
        }

        public Task Handle(Locked notification, CancellationToken cancellationToken) {
            _transactionManager.Add(new(notification.Lock, notification.User,
                                        ActionType.Lock, notification.Success));
            return Task.CompletedTask;
        }

        public Task Handle(Unlocked notification, CancellationToken cancellationToken) {
            _transactionManager.Add(new(notification.Lock, notification.User,
                                        ActionType.Unlock, notification.Success));
            return Task.CompletedTask;
        }
    }
}
