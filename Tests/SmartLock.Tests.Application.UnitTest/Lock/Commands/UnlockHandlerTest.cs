using FluentAssertions;

using SmartLock.Application.Services;
using SmartLock.Domain.Entities;
using SmartLock.Shared.Abstraction;

namespace SmartLock.Tests.Application.UnitTest.Lock.Commands {
    public sealed class UnlockHandlerTest : AbstractTest {
        readonly Mock<ILockRepository> _lockRepository = new();
        readonly Mock<IUserManagment> _userManagment = new();
        readonly Entities.User _user = Entities.User.Create("Test", "Test", "Test", new(1, "test", "test"));
        readonly Entities.Lock _lock = Entities.Lock.Create("test", "test", true);
        readonly UnlockHandler _handler;
        public UnlockHandlerTest() {
            _lockRepository.Setup(l => l.AddAsync(It.IsAny<Entities.Lock>())).Returns(Task.CompletedTask);
            _lockRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(_lock);
            _userManagment.Setup(u => u.CurrentUser).Returns(_user);
            _handler = new UnlockHandler(_lockRepository.Object, moqUnitOfWork.Object, _userManagment.Object);
        }

        [Fact]
        public async void Should_Unlock() {
            UnlockCommand request = new(1);

            Result result = await _handler.Handle(request, CancellationToken.None);

            result.Success.Should().BeTrue();
            _lock.IsLocked.Should().BeFalse();

            _lockRepository.Verify(l => l.Edit(_lock));
        }

        [Fact]
        public async void Should_Return_BadRequest() {
            UnlockCommand request = new(2);

            Result result = await _handler.Handle(request, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Code.Should().Be(ResultCode.BadRequest);
            _lock.IsLocked.Should().BeTrue();

            moqUnitOfWork.VerifyNoOtherCalls();
        }

        [Fact]
        public async void Should_Rerurns_Forbidden() {
            _lock.Edit(_lock.Name, _lock.Address, false);
            UnlockCommand request = new(1);

            Result result = await _handler.Handle(request, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Code.Should().Be(ResultCode.Forbidden);
            _lock.IsLocked.Should().BeTrue();

            moqUnitOfWork.VerifyNoOtherCalls();
        }
    }
}
