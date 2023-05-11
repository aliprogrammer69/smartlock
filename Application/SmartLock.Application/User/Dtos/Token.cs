namespace SmartLock.Application.User.Dtos {
    public sealed record Token(string Value, DateTime Expires);
}
