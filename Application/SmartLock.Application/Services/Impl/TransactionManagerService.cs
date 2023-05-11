using System.Collections.Concurrent;

using SmartLock.Domain.Repositories;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.Services.Impl {
    public sealed class TransactionManagerService : ITransactionManagerService {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ConcurrentBag<Entities.Transaction> _transactionPool = new ConcurrentBag<Entities.Transaction>();

        public TransactionManagerService(ITransactionRepository transactionRepository,
                                         IUnitOfWork unitOfWork) {
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Entities.Transaction transaction) {
            _transactionPool.Add(transaction);
            if (_transactionPool.Count >= 1)
                Persist();
        }

        #region private Methods
        private void Persist() {
            IEnumerable<Entities.Transaction> transactions = null;
            lock (_transactionPool) {
                if (_transactionPool.Count >= 1) {
                    transactions = _transactionPool.ToList();
                    _transactionPool.Clear();
                }
            }

            if (transactions?.Any() == true)
                Task.Run(async () => {
                    await _transactionRepository.AddRangeAsync(transactions);
                    await _unitOfWork.CommitAsync(dispacheEvent: false);
                }).Wait();
        }
        #endregion
    }
}
