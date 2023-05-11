namespace SmartLock.Application.Configurations.Models {
    public sealed class Audience {
        public string Secret { get; set; }
        public string Aud { get; set; }
        public string Iss { get; set; }
        public int? ExpiresInMinutes { get; set; }
        public int? RefreshTokenExpiresInMinutes { get; set; }
    }
}
