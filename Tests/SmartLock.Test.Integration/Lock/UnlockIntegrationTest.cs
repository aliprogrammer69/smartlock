using SmartLock.Shared.Abstraction;

namespace SmartLock.Test.Integration.Lock {
    public sealed class UnlockIntegrationTest : AbstractTest {
        readonly ILockRepository _lockRepository;
        readonly IMediator _mediator;
        public UnlockIntegrationTest(ServiceFixture service) : base(service) {
            _lockRepository = _services.GetService<ILockRepository>()!;
            _mediator = _services.GetService<IMediator>()!;
        }

        [Fact]
        public async void Should_Be_Unlock_Public_Lock() {
            Entities.Lock @lock = Entities.Lock.Create("Test", "Test", true);
            await _lockRepository.AddAsync(@lock);
            await _dbContext.SaveChangesAsync();

            UnlockCommand request = new(@lock.Id);
            Result result = await _mediator.Send(request);

            result.Success.Should().BeFalse();
            @lock.IsLocked.Should().BeFalse();
        }

        [Fact]
        public async void User_With_Admin_Role_Should_Be_Able_To_Unlock_All_Locks() {
            Entities.Lock @lock = Entities.Lock.Create("Test", "Test", false);
            await _lockRepository.AddAsync(@lock);
            await _dbContext.SaveChangesAsync();

            UnlockCommand request = new(@lock.Id);
            Result result = await _mediator.Send(request);

            result.Success.Should().BeTrue();
            @lock.IsLocked.Should().BeFalse();
        }

        [Fact]
        public async void Should_Returns_Forbidden() {
            Entities.Lock @lock = Entities.Lock.Create("Test", "Test", false);
            await _lockRepository.AddAsync(@lock);
            await _dbContext.SaveChangesAsync();

            _user.Edit(_user.UserName, new Entities.Role(2, "User", RoleKeys.User));

            UnlockCommand request = new(@lock.Id);
            Result result = await _mediator.Send(request);

            result.Success.Should().BeFalse();
            result.Code.Should().Be(ResultCode.Forbidden);
            @lock.IsLocked.Should().BeTrue();
        }
    }
}
