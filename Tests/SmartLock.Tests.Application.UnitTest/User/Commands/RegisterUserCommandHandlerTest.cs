using FluentAssertions;

using SmartLock.Application.Consts;
using SmartLock.Application.Services;
using SmartLock.Application.User.Commands;
using SmartLock.Application.User.Commands.Handlers;
using SmartLock.Application.User.Dtos;
using SmartLock.Domain.Entities;
using SmartLock.Shared.Abstraction.Services;

namespace SmartLock.Tests.Application.UnitTest.User.Commands {
    public sealed class RegisterUserCommandHandlerTest : AbstractTest {
        readonly Mock<IUserRepository> _userRepository = new();
        readonly Mock<IRoleRepository> _roleRepository = new();
        readonly Mock<ICryptoService> _cryptoService = new();
        readonly Mock<IAuthenticationProvider> _authenticationProvider = new();
        readonly RegisterUserCommand _request;
        readonly RegisterUserCommandHandler _handler;
        readonly Role _adminRole = new(1, "Admin", "admin");
        readonly string salt = "11111111111111111";
        public RegisterUserCommandHandlerTest() {
            _handler = new RegisterUserCommandHandler(_userRepository.Object, _roleRepository.Object, _cryptoService.Object, moqUnitOfWork.Object, _authenticationProvider.Object);
            _request = new RegisterUserCommand("Test", "Test", "admin");
            _roleRepository.Setup(r => r.GetByKey(_request.Role)).ReturnsAsync(_adminRole);
            _cryptoService.Setup(c => c.GenerateRandomString(It.IsAny<int>())).Returns(salt);
            _cryptoService.Setup(c => c.Hash(_request.Password, salt)).Returns("");

            _authenticationProvider.Setup(a => a.LoginAsync(It.IsAny<Entities.User>(), It.IsAny<IEnumerable<KeyValuePair<string, string>>>()))
                                   .ReturnsAsync(new AuthenticationResult(null, null));
        }

        [Fact]
        public async void Should_Returns_RoleNotFound() {
            RegisterUserCommand request = _request with {
                Role = "fake"
            };
            Result<AuthenticationResult> result = await _handler.Handle(request, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Messages.First().Should().Be(ErrorMessages.RoleNotFound);

            moqUnitOfWork.VerifyNoOtherCalls();
        }

        [Fact]
        public async void Should_Register_User() {
            Result<AuthenticationResult> result = await _handler.Handle(_request, CancellationToken.None);

            result.Success.Should().BeTrue();

            moqUnitOfWork.Verify(u => u.CommitAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()));
            _authenticationProvider.Verify(a => a.LoginAsync(It.IsAny<Entities.User>(), It.IsAny<IEnumerable<KeyValuePair<string, string>>>()));

            moqUnitOfWork.VerifyNoOtherCalls();
            _authenticationProvider.VerifyNoOtherCalls();
        }
    }
}
