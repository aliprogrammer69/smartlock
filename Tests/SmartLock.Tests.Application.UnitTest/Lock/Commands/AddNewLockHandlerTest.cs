using FluentAssertions;

namespace SmartLock.Tests.Application.UnitTest.Lock.Commands {
    public class AddNewLockHandlerTest : AbstractTest {
        [Fact]
        public async void Should_Create_Lock() {
            var lockRepoMoq = new Mock<ILockRepository>();
            lockRepoMoq.Setup(l => l.AddAsync(It.IsAny<Entities.Lock>())).Returns(Task.CompletedTask);

            AddNewLockHandler addLockHander = new(lockRepoMoq.Object, moqUnitOfWork.Object);
            AddNewLockCommand request = new("test", "test");
            Result result = await addLockHander.Handle(request, CancellationToken.None);

            result.Success.Should().BeTrue();

            lockRepoMoq.Verify(l => l.AddAsync(It.IsAny<Entities.Lock>()));
            moqUnitOfWork.Verify(u => u.CommitAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()));

            lockRepoMoq.VerifyNoOtherCalls();
            moqUnitOfWork.VerifyNoOtherCalls();
        }
    }
}
