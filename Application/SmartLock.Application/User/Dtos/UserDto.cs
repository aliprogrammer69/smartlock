namespace SmartLock.Application.User.Dtos {
    public sealed record UserDto(ulong id, string UserName, RoleDto Role, DateTime? LastLoginDate = null);
}
