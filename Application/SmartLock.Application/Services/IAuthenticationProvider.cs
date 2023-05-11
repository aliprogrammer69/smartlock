using System.Security.Claims;

using SmartLock.Application.User.Dtos;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.Services {
    public interface IAuthenticationProvider {
        Task<AuthenticationResult> LoginAsync(Entities.User userInfo, IEnumerable<KeyValuePair<string, string>> claims = null);
        bool IsValidToken(string token, out ClaimsPrincipal principal);

        Token GenerateAccessToken(Entities.User userInfo, IEnumerable<KeyValuePair<string, string>> claims = null);
        Token GenerateRefreshToken();

    }
}
