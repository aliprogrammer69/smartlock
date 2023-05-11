using MediatR;

using SmartLock.Application.Consts;
using SmartLock.Application.Services;
using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction;
using SmartLock.Shared.Abstraction.Models;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.Lock.Commands.Handlers {
    public sealed class UnlockHandler : IRequestHandler<UnlockCommand, Result> {
        private readonly ILockRepository _lockRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagment _userManagment;

        public UnlockHandler(ILockRepository lockRepository,
                             IUnitOfWork unitOfWork,
                             IUserManagment userManagment) {
            _lockRepository = lockRepository;
            _unitOfWork = unitOfWork;
            _userManagment = userManagment;
        }

        public async Task<Result> Handle(UnlockCommand request, CancellationToken cancellationToken) {
            Entities.Lock item = await _lockRepository.GetAsync(request.lockId);
            if (item is null)
                return Result.BadRequest(ErrorMessages.LockNotFound);


            bool unLockResult = true;
            unLockResult = item.UnLock(_userManagment.CurrentUser);
            if (unLockResult) {
                _lockRepository.Edit(item);
                await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);
            }

            return new Result(unLockResult ? ResultCode.Ok : ResultCode.Forbidden, null);
        }
    }
}
