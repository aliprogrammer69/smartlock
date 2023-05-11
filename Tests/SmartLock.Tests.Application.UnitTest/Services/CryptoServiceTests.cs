using FluentAssertions;

using SmartLock.Shared.Abstraction.Services;
using SmartLock.Shared.Services;

namespace SmartLock.Tests.Application.UnitTest.Services {
    public sealed class CryptoServiceTests : AbstractTest {
        readonly ICryptoService _cryptoService = new CryptoService();

        public void Should_Generate_FixedSize_Random_String() {
            byte length = 32;
            string result = _cryptoService.GenerateRandomString(length);

            result.Length.Should().Be(length);
        }
    }
}
