using FluentAssertions;

using SmartLock.Application.Consts;
using SmartLock.Application.Services;
using SmartLock.Application.User.Commands;
using SmartLock.Application.User.Commands.Handlers;
using SmartLock.Application.User.Dtos;
using SmartLock.Shared.Abstraction.Services;

namespace SmartLock.Tests.Application.UnitTest.User.Commands {
    public class LoginUserCommandHandlerTest : AbstractTest {
        readonly Mock<IUserRepository> _userRepository = new ();
        readonly Mock<ICryptoService> _cryptoService = new ();
        readonly Mock<IAuthenticationProvider> _authenticationProvider = new ();
        readonly LoginUserCommandHandler _handler;
        readonly LoginUserCommand _request;
        public LoginUserCommandHandlerTest() {
            _handler = new LoginUserCommandHandler(_userRepository.Object, _cryptoService.Object, _authenticationProvider.Object);
            _request = new LoginUserCommand("Test", "Test");
        }

        public async void Should_Returns_Forbidden_For_Invalid_UserName() {
            Entities.User nullUser = null!;
            _userRepository.Setup(u => u.GetByUsernameAsync("Invalid")).ReturnsAsync(nullUser);

            Result<AuthenticationResult> result = await _handler.Handle(_request, CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Messages.Should().HaveCountGreaterThan(0);
            result.Messages.Contains(ErrorMessages.UsernameOrPasswordNotCurrent).Should().BeTrue();

            _authenticationProvider.VerifyNoOtherCalls();
        }


    }
}
