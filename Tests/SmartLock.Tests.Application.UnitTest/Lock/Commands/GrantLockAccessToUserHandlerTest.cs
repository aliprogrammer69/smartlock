using FluentAssertions;

using SmartLock.Application.Consts;
using SmartLock.Application.Services;

namespace SmartLock.Tests.Application.UnitTest.Lock.Commands {
    public sealed class GrantLockAccessToUserHandlerTest : AbstractTest {
        readonly Mock<ILockRepository> _lockRepository = new();
        readonly Mock<IUserManagment> _userManagment = new();
        readonly Mock<IUserRepository> _userRepository = new();
        readonly Entities.User _user = Entities.User.Create("Test", "Test", "Test", new(1, "Admin", "admin"));
        readonly Entities.Lock _lock = Entities.Lock.Create("test", "test", true);
        readonly GrantLockAccessToUserHandler _handler;
        public GrantLockAccessToUserHandlerTest() {
            _lockRepository.Setup(l => l.AddAsync(It.IsAny<Entities.Lock>())).Returns(Task.CompletedTask);
            _userManagment.Setup(u => u.CurrentUser).Returns(_user);
            _lockRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(_lock);
            _userRepository.Setup(u => u.GetByIdAsync(1)).ReturnsAsync(_user);
            _handler = new GrantLockAccessToUserHandler(_lockRepository.Object, _userRepository.Object,
                                                        moqUnitOfWork.Object, _userManagment.Object);
        }

        [Fact]
        public async void Should_Grant_Access_To_User() {
            GrantLockAccessToUserCommand request = new(1, 1);
            Result result = await _handler.Handle(request, CancellationToken.None);

            result.Success.Should().BeTrue();

            _lockRepository.Verify(l => l.Edit(_lock));
            moqUnitOfWork.Verify(l => l.CommitAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async void Should_Returns_UserNotFound() {
            GrantLockAccessToUserCommand request = new(2, 1);
            Result result = await _handler.Handle(request, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Messages.Should().HaveCount(1);
            result.Messages.First().Should().Be(ErrorMessages.UserNotFound);

            moqUnitOfWork.VerifyNoOtherCalls();
        }

        [Fact]
        public async void Should_Returns_LockNotFound() {
            GrantLockAccessToUserCommand request = new(1, 2);
            Result result = await _handler.Handle(request, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Messages.Should().HaveCount(1);
            result.Messages.First().Should().Be(ErrorMessages.LockNotFound);

            moqUnitOfWork.VerifyNoOtherCalls();
        }
    }
}
