namespace SmartLock.Application.Configurations.Models {
    public sealed class RefreshTokenConfig {
        public sbyte TokenLength { get; set; } = 32;
        public sbyte ValidFormDays { get; set; } = 5;
    }
}
