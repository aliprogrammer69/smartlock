namespace SmartLock.Shared.Abstraction.Services {
    public interface ICryptoService {
        string Hash(string data, string salt);
        string GenerateRandomString(int length);
    }
}
