namespace SmartLock.Application.User.Dtos {
    public sealed class AuthenticationResult {
        public AuthenticationResult(Token accessToken, Token refreshToken = null) {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public Token AccessToken { get; set; }
        public Token RefreshToken { get; set; }
    }
}
