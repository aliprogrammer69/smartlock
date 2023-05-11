using Moq;

using SmartLock.Domain.Repositories;

namespace SmartLock.Tests.Application.UnitTest {
    public abstract class AbstractTest {
        protected Mock<IUnitOfWork> moqUnitOfWork = new();
        public AbstractTest() {
            moqUnitOfWork.Setup(u => u.CommitAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
        }
    }
}