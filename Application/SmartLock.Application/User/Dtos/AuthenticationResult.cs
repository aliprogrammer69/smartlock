namespace SmartLock.Application.User.Dtos {
    public sealed class AuthenticationResult {
        public AuthenticationResult(UserDto user, Token accessToken, Token refreshToken = null) {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            User = user;
        }

        public Token AccessToken { get; set; }
        public Token RefreshToken { get; set; }
        public UserDto User { get; set; }
    }
}
