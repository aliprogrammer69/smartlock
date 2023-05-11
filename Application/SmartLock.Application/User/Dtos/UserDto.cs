namespace SmartLock.Application.User.Dtos {
    public sealed record UserDto(string UserName, RoleDto Role, DateTime? LastLoginDate = null);
}
