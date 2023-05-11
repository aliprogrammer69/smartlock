using MediatR;

using SmartLock.Application.Consts;
using SmartLock.Application.Services;
using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction;
using SmartLock.Shared.Abstraction.Models;

using Entities = SmartLock.Domain.Entities;
namespace SmartLock.Application.Lock.Commands.Handlers {
    public sealed class DenyLockAccessFromUserHandler : IRequestHandler<DenyLockAccessFromUserCommand, Result> {
        private readonly IUserRepository _userRepository;
        private readonly ILockRepository _lockRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagment _userManagment;

        public DenyLockAccessFromUserHandler(IUserRepository userRepository,
                                             ILockRepository lockRepository,
                                             IUnitOfWork unitOfWork,
                                             IUserManagment userManagment) {
            _userRepository = userRepository;
            _lockRepository = lockRepository;
            _unitOfWork = unitOfWork;
            _userManagment = userManagment;
        }

        public async Task<Result> Handle(DenyLockAccessFromUserCommand request, CancellationToken cancellationToken) {
            Entities.User user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
                return Result.BadRequest(ErrorMessages.UserNotFound);

            Entities.Lock @lock = await _lockRepository.GetAsync(request.LockId);
            if (@lock is null)
                return Result.BadRequest(ErrorMessages.LockNotFound);

            if (!string.Equals(_userManagment.CurrentUser.Role.Key, RoleKeys.Admin) &&
                !await _userRepository.HasAccessToLock(_userManagment.CurrentUser.Id, request.LockId))
                return new Result(ResultCode.Forbidden, null);

            @lock.DenyAccess(user);
            _lockRepository.Edit(@lock);
            await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);
            return Result.Successed();
        }
    }
}
