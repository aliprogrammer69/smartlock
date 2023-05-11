namespace SmartLock.Test.Integration.Lock {
    public class AddLockIntegrationTest : AbstractTest {
        readonly IMediator _mediator;
        readonly ILockRepository _lockRepository;
        public AddLockIntegrationTest(ServiceFixture service) : base(service) {
            _mediator = _services.GetService<IMediator>()!;
            _lockRepository = _services.GetService<ILockRepository>()!;
        }

        [Fact]
        public async void Should_Add_New_lock() {
            AddNewLockCommand request = new ("Test", "Address");
            Result result = await _mediator.Send(request);

            Entities.Lock @lock = _dbContext.Locks.FirstOrDefault()!;

            result.Success.Should().BeTrue();
            @lock.Should().NotBeNull();
            @lock.Name.Should().Be("Test");
        }

        [Theory]
        [MemberData(nameof(BadAddLockRequests))]
        public async void Should_Returns_BadRequest(AddNewLockCommand request) {
            Result result = await _mediator.Send(request);

            Entities.Lock @lock = _dbContext.Locks.FirstOrDefault()!;
            result.Success.Should().BeFalse();
            @lock.Should().BeNull();
        }

        public static IEnumerable<object[]> BadAddLockRequests() {
            yield return new object[] { new AddNewLockCommand("", "Address") };
            yield return new object[] { new AddNewLockCommand("Name", "") };
        }
    }
}
