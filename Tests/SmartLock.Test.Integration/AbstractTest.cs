using SmartLock.Application.Services;

namespace SmartLock.Test.Integration {
    public abstract class AbstractTest : IClassFixture<ServiceFixture> {
        protected readonly IServiceProvider _services;
        protected readonly IUserManagment _userManagment;
        protected readonly SmartLockDbContext _dbContext;
        protected readonly Entities.User _user;
        public AbstractTest(ServiceFixture service) {
            _services = service.ServiceProvider.CreateScope().ServiceProvider;
            _dbContext = _services.GetService<SmartLockDbContext>()!;
            _userManagment = _services.GetService<IUserManagment>()!;
            _user = Entities.User.Create("Test", "Test", "Test", new Entities.Role(1, "Admin", "admin"));
            Initialize();
        }

        private async void Initialize() {
            IUserRepository userRepository = _services.GetService<IUserRepository>()!;
            await userRepository.AddAsync(_user);
            await _dbContext.SaveChangesAsync();
            await _userManagment.Authenticate(_user.Id);
        }
    }
}
