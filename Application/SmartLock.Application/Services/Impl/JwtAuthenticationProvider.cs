using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using SmartLock.Application.Configurations.Models;
using SmartLock.Application.Consts;
using SmartLock.Application.User.Dtos;
using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction.Services;

namespace SmartLock.Application.Services.Impl {
    public sealed class JwtAuthenticationProvider : IAuthenticationProvider {
        private readonly Audience _audience;
        private readonly RefreshTokenConfig _refreshTokenConfig;
        private readonly ICryptoService _cryptoService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public JwtAuthenticationProvider(Audience audience,
                                         RefreshTokenConfig refreshTokenConfig,
                                         ICryptoService cryptoService,
                                         IUserRepository userRepository,
                                         IUnitOfWork unitOfWork) {
            _audience = audience;
            _refreshTokenConfig = refreshTokenConfig;
            _cryptoService = cryptoService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthenticationResult> LoginAsync(Domain.Entities.User userInfo, IEnumerable<KeyValuePair<string, string>> claims = null) {
            AuthenticationResult result = new(GenerateAccessToken(userInfo, claims), GenerateRefreshToken());
            userInfo.Login(result.RefreshToken.Value, result.RefreshToken.Expires);
            _userRepository.Edit(userInfo);
            await _unitOfWork.CommitAsync();
            return result;
        }

        public bool IsValidToken(string token, out ClaimsPrincipal principal) {
            try {
                SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_audience.Secret));
                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = _audience.Iss,
                    ValidateAudience = true,
                    ValidAudience = _audience.Aud,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = false,
                };

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                principal = handler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                return principal != null;
            }
            catch {
                principal = null;
                return false;
            }
        }

        public Token GenerateAccessToken(Domain.Entities.User userInfo, IEnumerable<KeyValuePair<string, string>> claims = null) {
            DateTime now = DateTime.Now;

            List<Claim> claimsList = new List<Claim>
            {
                    new Claim(ClaimNames.ClaimNames_Sub, userInfo.UserName,ClaimValueTypes.String),
                    new Claim(ClaimNames.ClaimNames_Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimNames.ClaimNames_Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                    new Claim(ClaimNames.ClaimNames_UserId, userInfo.Id.ToString(), ClaimValueTypes.Integer64),
                    new Claim(ClaimNames.ClaimNames_Role, userInfo.Role.Key,ClaimValueTypes.String)
            };
            if (claims != null) {
                foreach (KeyValuePair<string, string> claim in claims) {
                    claimsList.Add(new Claim(claim.Key, claim.Value, ClaimValueTypes.String));
                }
            }

            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_audience.Secret));

            DateTime expire = now.Add(TimeSpan.FromMinutes(_audience.ExpiresInMinutes ?? 2));

            JwtSecurityToken jwt = new JwtSecurityToken(
                audience: _audience.Aud,
                issuer: _audience.Iss,
                claims: claimsList,
                notBefore: now,
                expires: expire,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new(new JwtSecurityTokenHandler().WriteToken(jwt), expire);
        }

        public Token GenerateRefreshToken() {
            string refreshToken = _cryptoService.GenerateRandomString(_refreshTokenConfig.TokenLength);
            return new(refreshToken, DateTime.Now.AddDays(_refreshTokenConfig.ValidFormDays));
        }
    }
}
