namespace SmartLock.Application.Lock.Dtos {
    public sealed record LockDto(string Name, string Address, bool IsPublic, bool IsLock);
}
