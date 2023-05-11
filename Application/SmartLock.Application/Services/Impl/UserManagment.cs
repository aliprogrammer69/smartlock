using SmartLock.Domain.Repositories;

using Entities = SmartLock.Domain.Entities;
namespace SmartLock.Application.Services.Impl {
    public sealed class UserManagment : IUserManagment {
        private readonly IUserRepository _userRepository;

        public UserManagment(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public Entities.User CurrentUser { get; private set; }

        public async Task<bool> Authenticate(ulong userId) {
            CurrentUser = await _userRepository.GetByIdAsync(userId);
            return CurrentUser is not null;
        }
    }
}
