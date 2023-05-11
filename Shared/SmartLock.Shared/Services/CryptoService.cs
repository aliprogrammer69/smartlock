using System.Security.Cryptography;
using System.Text;

using SmartLock.Shared.Abstraction.Services;

namespace SmartLock.Shared.Services {
    public sealed class CryptoService : ICryptoService {
        public string GenerateRandomString(int length) {
            byte[] buffer = new byte[length];
            using (RandomNumberGenerator generator = RandomNumberGenerator.Create()) {
                generator.GetBytes(buffer);
                return Convert.ToBase64String(buffer);
            }
        }

        public string Hash(string data, string salt) {
            var hashManager = SHA512.Create();
            var hashData = hashManager.ComputeHash(Encoding.UTF8.GetBytes(salt + data));
            return Convert.ToBase64String(hashData);
        }
    }
}
