using MediatR;

using SmartLock.Application.Consts;
using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction.Models;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.Lock.Commands.Handlers {
    public sealed class EditLockHandler : IRequestHandler<EditLockCommand, Result> {
        private readonly ILockRepository _lockRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EditLockHandler(ILockRepository lockRepository,
                               IUnitOfWork unitOfWork) {
            _lockRepository = lockRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(EditLockCommand request, CancellationToken cancellationToken) {
            Entities.Lock item = await _lockRepository.GetAsync(request.Id);
            if (item is null)
                return Result.BadRequest(ErrorMessages.LockNotFound);

            item.Edit(request.Name,
                      request.Address,
                      request.IsPublic);

            _lockRepository.Edit(item);
            await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return Result.Successed();
        }
    }
}
