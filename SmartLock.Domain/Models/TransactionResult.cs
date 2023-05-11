using SmartLock.Domain.Entities;

namespace SmartLock.Domain.Models {
    public sealed class TransactionResult {
        public TransactionResult(IEnumerable<Transaction> transactions,
                                 ulong totalCount,
                                 int first, int after) {
            Transactions = transactions;
            TotalCount = totalCount;
            First = first;
            After = after;
        }

        public IEnumerable<Transaction> Transactions { get; private set; }
        public ulong TotalCount { get; private set; }
        public int First { get; private set; }
        public int After { get; private set; }
    }
}
