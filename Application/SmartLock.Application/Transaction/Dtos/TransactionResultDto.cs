namespace SmartLock.Application.Transaction.Dtos {
    public sealed record TransactionResultDto(IEnumerable<TransactionDto> Transactions, ulong TotalCount, int First, int After);
}
