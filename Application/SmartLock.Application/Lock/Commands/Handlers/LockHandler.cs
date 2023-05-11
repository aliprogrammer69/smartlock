using MediatR;

using SmartLock.Application.Consts;
using SmartLock.Application.Services;
using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction.Models;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.Lock.Commands.Handlers {
    public sealed class LockHandler : IRequestHandler<LockCommand, Result> {
        private readonly ILockRepository _lockRepository;
        private readonly IUserManagment _userManagment;
        private readonly IUnitOfWork _unitOfWork;

        public LockHandler(ILockRepository lockRepository,
                           IUserManagment userManagment,
                           IUnitOfWork unitOfWork) {
            _lockRepository = lockRepository;
            _userManagment = userManagment;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(LockCommand request, CancellationToken cancellationToken) {
            Entities.Lock item = await _lockRepository.GetAsync(request.Id);
            if (item is null)
                return Result.BadRequest(ErrorMessages.LockNotFound);

            item.CloseTheDoor(_userManagment.CurrentUser);
            _lockRepository.Edit(item);
            await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);
            return Result.Successed();
        }
    }
}
