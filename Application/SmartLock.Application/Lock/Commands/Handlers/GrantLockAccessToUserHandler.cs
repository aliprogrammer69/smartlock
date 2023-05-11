using MediatR;

using SmartLock.Application.Consts;
using SmartLock.Application.Services;
using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction;
using SmartLock.Shared.Abstraction.Models;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.Lock.Commands.Handlers {
    public sealed class GrantLockAccessToUserHandler : IRequestHandler<GrantLockAccessToUserCommand, Result> {
        private readonly ILockRepository _lockRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagment _userManagment;

        public GrantLockAccessToUserHandler(ILockRepository lockRepository,
                                            IUserRepository userRepository,
                                            IUnitOfWork unitOfWork,
                                            IUserManagment userManagment) {
            _lockRepository = lockRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userManagment = userManagment;
        }

        public async Task<Result> Handle(GrantLockAccessToUserCommand request, CancellationToken cancellationToken) {
            Entities.User user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
                return Result.BadRequest(ErrorMessages.UserNotFound);

            Entities.Lock @lock = await _lockRepository.GetAsync(request.LockId);
            if (@lock is null)
                return Result.BadRequest(ErrorMessages.LockNotFound);

            if (!string.Equals(_userManagment.CurrentUser.Role.Key, RoleKeys.Admin) &&
                !await _userRepository.HasAccessToLock(_userManagment.CurrentUser.Id, request.LockId))
                return new Result(ResultCode.Forbidden, null);

            @lock.GrandAccess(user);
            _lockRepository.Edit(@lock);
            await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);
            return Result.Successed();
        }
    }
}
